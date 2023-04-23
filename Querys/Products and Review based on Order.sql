Select Products.ProductId, ProductName, QuantityPerUnit, Products.UnitPrice, Quantity, Discount, Reviews.Rating, Reviews.Comment 
from Products 
LEFT JOIN OrderDetails ON Products.ProductId = OrderDetails.ProductId 
LEFT JOIN Reviews ON OrderDetails.ProductId = Reviews.ProductId
WHERE OrderId = 10267 
ORDER BY OrderDetails.OrderDetailsId