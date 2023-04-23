SELECT ProductName, AVG(Rating) AS AvgRating FROM Products
LEFT JOIN Reviews ON Products.ProductId = Reviews.ProductId
GROUP BY Products.ProductId, Products.ProductName
ORDER BY Products.ProductId
