DELETE r1
FROM Reviews r1
INNER JOIN (
  SELECT CustomerId, ProductId
  FROM Reviews
  GROUP BY CustomerId, ProductId
  HAVING COUNT(*) > 1
) r2 ON r1.CustomerId = r2.CustomerId AND r1.ProductId = r2.ProductId
WHERE r1.DateTime NOT IN (
  SELECT MIN(DateTime)
  FROM Reviews
  WHERE CustomerId = r1.CustomerId AND ProductId = r1.ProductId
);