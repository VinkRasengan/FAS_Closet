// This file defines the OrderDetailManager class, which handles order detail-related operations.

using System;
using System.Collections.Generic;
using System.Data;
using FASCloset.Data;
using FASCloset.Models;

namespace FASCloset.Services
{
    /// <summary>
    /// Manages order detail operations including creation, retrieval, and updating
    /// </summary>
    public static class OrderDetailManager
    {
        // SQL parameter constants
        private const string PARAM_ORDER_ID = "@OrderID";
        private const string PARAM_PRODUCT_ID = "@ProductID";
        private const string PARAM_QUANTITY = "@Quantity";
        private const string PARAM_UNIT_PRICE = "@UnitPrice";
        private const string PARAM_ORDER_DETAIL_ID = "@OrderDetailID";

        /// <summary>
        /// Adds a new order detail to the database
        /// </summary>
        /// <param name="orderDetail">Order detail object to add</param>
        /// <returns>ID of the newly added order detail, or -1 on failure</returns>
        public static int AddOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                string query = @"
                    INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice)
                    VALUES (@OrderID, @ProductID, @Quantity, @UnitPrice);
                    SELECT last_insert_rowid();";
                    
                var parameters = new Dictionary<string, object>
                {
                    { PARAM_ORDER_ID, orderDetail.OrderID },
                    { PARAM_PRODUCT_ID, orderDetail.ProductID },
                    { PARAM_QUANTITY, orderDetail.Quantity },
                    { PARAM_UNIT_PRICE, orderDetail.UnitPrice }
                };
                
                int orderDetailId = DataAccessHelper.ExecuteScalar<int>(query, parameters);
                
                // Update the Product stock quantity
                if (orderDetailId > 0)
                {
                    UpdateProductStock(orderDetail.ProductID, orderDetail.Quantity);
                }
                
                return orderDetailId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding order detail: {ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// Updates an existing order detail
        /// </summary>
        /// <param name="orderDetail">Updated order detail information</param>
        /// <returns>True if update was successful</returns>
        public static bool UpdateOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                // Get the current order detail to calculate stock difference
                OrderDetail? currentOrderDetail = GetOrderDetailById(orderDetail.OrderDetailID);
                if (currentOrderDetail == null)
                    return false;
                
                // Update the order detail
                string query = @"
                    UPDATE OrderDetails 
                    SET ProductID = @ProductID,
                        Quantity = @Quantity,
                        UnitPrice = @UnitPrice
                    WHERE OrderDetailID = @OrderDetailID";
                    
                var parameters = new Dictionary<string, object>
                {
                    { PARAM_ORDER_DETAIL_ID, orderDetail.OrderDetailID },
                    { PARAM_PRODUCT_ID, orderDetail.ProductID },
                    { PARAM_QUANTITY, orderDetail.Quantity },
                    { PARAM_UNIT_PRICE, orderDetail.UnitPrice }
                };
                
                int rowsAffected = DataAccessHelper.ExecuteNonQuery(query, parameters);
                
                if (rowsAffected > 0)
                {
                    // If the product changed, restore the old product's stock and reduce the new product's stock
                    if (currentOrderDetail.ProductID != orderDetail.ProductID)
                    {
                        // Restore old product stock (add the quantity back)
                        UpdateProductStock(currentOrderDetail.ProductID, -currentOrderDetail.Quantity);
                        
                        // Reduce new product stock
                        UpdateProductStock(orderDetail.ProductID, orderDetail.Quantity);
                    }
                    // Otherwise, only update stock if the quantity changed
                    else if (currentOrderDetail.Quantity != orderDetail.Quantity)
                    {
                        // Calculate the difference in quantity and update stock accordingly
                        int quantityDifference = orderDetail.Quantity - currentOrderDetail.Quantity;
                        if (quantityDifference != 0)
                        {
                            UpdateProductStock(orderDetail.ProductID, quantityDifference);
                        }
                    }
                    
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating order detail: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Deletes an order detail from the database
        /// </summary>
        /// <param name="orderDetailId">ID of the order detail to delete</param>
        /// <returns>True if deletion was successful</returns>
        public static bool DeleteOrderDetail(int orderDetailId)
        {
            try
            {
                // Get the current order detail to restore stock
                OrderDetail? orderDetail = GetOrderDetailById(orderDetailId);
                if (orderDetail == null)
                    return false;
                
                string query = "DELETE FROM OrderDetails WHERE OrderDetailID = @OrderDetailID";
                var parameters = new Dictionary<string, object>
                {
                    { PARAM_ORDER_DETAIL_ID, orderDetailId }
                };
                
                int rowsAffected = DataAccessHelper.ExecuteNonQuery(query, parameters);
                
                if (rowsAffected > 0)
                {
                    // Restore stock to inventory (negate the quantity to add it back)
                    UpdateProductStock(orderDetail.ProductID, -orderDetail.Quantity);
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting order detail: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets a specific order detail by its ID
        /// </summary>
        /// <param name="orderDetailId">ID of the order detail to retrieve</param>
        /// <returns>Order detail object or null if not found</returns>
        public static OrderDetail? GetOrderDetailById(int orderDetailId)
        {
            string query = @"
                SELECT od.OrderDetailID, od.OrderID, od.ProductID, p.ProductName, 
                       od.Quantity, od.UnitPrice
                FROM OrderDetails od
                JOIN Product p ON od.ProductID = p.ProductID
                WHERE od.OrderDetailID = @OrderDetailID";
                
            var parameters = new Dictionary<string, object>
            {
                { PARAM_ORDER_DETAIL_ID, orderDetailId }
            };
            
            return DataAccessHelper.ExecuteReaderSingle(query, MapOrderDetailFromReader, parameters);
        }

        /// <summary>
        /// Gets all details for a specific order
        /// </summary>
        /// <param name="orderId">ID of the order</param>
        /// <returns>List of order details</returns>
        public static List<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            string query = @"
                SELECT od.OrderDetailID, od.OrderID, od.ProductID, p.ProductName, 
                       od.Quantity, od.UnitPrice
                FROM OrderDetails od
                JOIN Product p ON od.ProductID = p.ProductID
                WHERE od.OrderID = @OrderID";
                
            var parameters = new Dictionary<string, object>
            {
                { PARAM_ORDER_ID, orderId }
            };
            
            return DataAccessHelper.ExecuteReader(query, MapOrderDetailFromReader, parameters);
        }

        /// <summary>
        /// Updates the stock quantity of a product after an order is placed or updated
        /// </summary>
        /// <param name="productId">ID of the product to update</param>
        /// <param name="quantityToReduce">Quantity to reduce from the stock (negative to increase)</param>
        private static void UpdateProductStock(int productId, int quantityToReduce)
        {
            // Delegate to the inventory manager
            InventoryManager.UpdateStock(productId, 
                ProductManager.GetProductById(productId)?.Stock - quantityToReduce ?? 0);
        }

        /// <summary>
        /// Maps a database record to an OrderDetail object
        /// </summary>
        /// <param name="reader">Data reader containing order detail data</param>
        /// <returns>Populated OrderDetail object</returns>
        private static OrderDetail MapOrderDetailFromReader(IDataReader reader)
        {
            return new OrderDetail
            {
                OrderDetailID = reader.GetInt32(reader.GetOrdinal("OrderDetailID")),
                OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice"))
                // TotalPrice is now calculated automatically from Quantity and UnitPrice
            };
        }

        /// <summary>
        /// Gets all products in an order with their quantities and prices
        /// </summary>
        /// <param name="orderId">ID of the order</param>
        /// <returns>List of products in the order with quantity information</returns>
        public static List<Product> GetProductsInOrder(int orderId)
        {
            string query = @"
                SELECT p.*, od.Quantity, od.UnitPrice
                FROM OrderDetails od
                JOIN Product p ON od.ProductID = p.ProductID
                WHERE od.OrderID = @OrderID";
                
            var parameters = new Dictionary<string, object>
            {
                { PARAM_ORDER_ID, orderId }
            };
            
            return DataAccessHelper.ExecuteReader(query, reader =>
            {
                var product = new Product
                {
                    Id = reader.GetInt32(reader.GetOrdinal("ProductID")),
                    Name = reader.GetString(reader.GetOrdinal("ProductName")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                    Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                    OrderQuantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                    OrderUnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice"))
                };
                
                if (!reader.IsDBNull(reader.GetOrdinal("ManufacturerID")))
                {
                    product.ManufacturerId = reader.GetInt32(reader.GetOrdinal("ManufacturerID"));
                }
                
                return product;
            }, parameters);
        }

        /// <summary>
        /// Gets the total item count across all order details for an order
        /// </summary>
        /// <param name="orderId">ID of the order</param>
        /// <returns>Total item count</returns>
        public static int GetTotalItemCount(int orderId)
        {
            string query = "SELECT SUM(Quantity) FROM OrderDetails WHERE OrderID = @OrderID";
            var parameters = new Dictionary<string, object>
            {
                { PARAM_ORDER_ID, orderId }
            };
            
            var result = DataAccessHelper.ExecuteScalar<object>(query, parameters);
            return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
        }
    }
}
