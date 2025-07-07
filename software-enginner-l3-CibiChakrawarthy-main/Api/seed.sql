PRAGMA
foreign_keys = OFF;
BEGIN
TRANSACTION;

-- Drop any existing tables
DROP TABLE IF EXISTS OrderItems;
DROP TABLE IF EXISTS Orders;
DROP TABLE IF EXISTS Products;

-- Create Products table
CREATE TABLE Products
(
    Id        INTEGER PRIMARY KEY,
    Name      TEXT NOT NULL,
    UnitCost  REAL NOT NULL,
    UnitPrice REAL NOT NULL
);

-- Create Orders table
CREATE TABLE Orders
(
    Id        INTEGER PRIMARY KEY,
    Status    TEXT NOT NULL,
    OrderDate TEXT NOT NULL
);

-- Create OrderItems table
CREATE TABLE OrderItems
(
    OrderId   INTEGER NOT NULL,
    ProductId INTEGER NOT NULL,
    Quantity  INTEGER NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders (Id),
    FOREIGN KEY (ProductId) REFERENCES Products (Id)
);

-- Insert 50 deterministic supplement products
INSERT INTO Products(Id, Name, UnitCost, UnitPrice)
VALUES (1, 'Ashwagandha KSM-66®', 10.00, 20.00),
       (2, 'Biotin', 5.00, 8.00),
       (3, 'Iron', 2.00, 4.00),
       (4, 'Magnesium', 3.00, 6.00),
       (5, 'Omega 3', 1.00, 3.00),
       (6, 'Vitamin C', 4.00, 7.00),
       (7, 'Vitamin D3', 6.00, 10.00),
       (8, 'Calcium', 2.00, 5.00),
       (9, 'Zinc', 1.50, 3.00),
       (10, 'Vitamin B12', 7.00, 12.00),
       (11, 'Vitamin B6', 3.00, 5.00),
       (12, 'Folic Acid', 1.00, 3.00),
       (13, 'Magnesium Citrate', 3.00, 5.00),
       (14, 'Magnesium Glycinate', 4.00, 7.00),
       (15, 'Omega 6', 2.00, 4.00),
       (16, 'Omega 9', 2.50, 5.00),
       (17, 'Probiotic Blend', 8.00, 15.00),
       (18, 'Turmeric Curcumin', 10.00, 18.00),
       (19, 'CoQ10', 7.00, 14.00),
       (20, 'L-Glutamine', 5.00, 9.00),
       (21, 'L-Arginine', 6.00, 11.00),
       (22, 'Creatine Monohydrate', 3.00, 6.00),
       (23, 'Collagen Peptides', 8.00, 16.00),
       (24, 'Hyaluronic Acid', 5.00, 10.00),
       (25, 'Melatonin', 2.00, 4.00),
       (26, 'Milk Thistle', 7.00, 13.00),
       (27, 'Spirulina', 9.00, 17.00),
       (28, 'Chlorella', 8.00, 14.00),
       (29, 'Green Tea Extract', 6.00, 12.00),
       (30, 'Garlic Extract', 4.00, 8.00),
       (31, 'Elderberry', 5.00, 10.00),
       (32, 'Echinacea', 6.00, 12.00),
       (33, 'Saw Palmetto', 7.00, 13.00),
       (34, 'Ginkgo Biloba', 5.00, 11.00),
       (35, 'Ginseng', 8.00, 15.00),
       (36, 'Ashwagandha Liquid', 12.00, 20.00),
       (37, 'Biotin Gummies', 5.00, 8.00),
       (38, 'Iron Chelate', 3.00, 6.00),
       (39, 'Magnesium Oxide', 4.00, 7.00),
       (40, 'Omega 3-6-9', 2.00, 5.00),
       (41, 'Vitamin K2', 3.00, 6.00),
       (42, 'Vitamin E', 4.00, 8.00),
       (43, 'Selenium', 2.00, 5.00),
       (44, 'Chromium Picolinate', 3.00, 6.00),
       (45, 'Alpha Lipoic Acid', 4.00, 8.00),
       (46, 'Glucosamine', 6.00, 12.00),
       (47, 'Chondroitin', 5.00, 10.00),
       (48, 'MSM', 3.00, 6.00),
       (49, 'L-Carnitine', 4.00, 9.00),
       (50, 'L-Theanine', 2.00, 5.00);

-- Generate 100 orders with mixed statuses and evenly distributed dates
WITH RECURSIVE
    nums(n) AS (SELECT 1
                UNION ALL
                SELECT n + 1
                FROM nums
                WHERE n < 100),
    orders AS (SELECT n       AS Id,
                      CASE
                          WHEN n % 3 = 0 THEN 'Cancelled'
                          ELSE 'Complete'
                          END AS Status,
    date ('2025-01-01', round((n - 1) * 181.0 / 99.0) || ' days'
    ) AS OrderDate, ((n - 1) % 5) + 1 AS ItemCount
FROM nums
    )
INSERT
INTO Orders(Id, Status, OrderDate)
SELECT Id, Status, OrderDate
FROM orders;

-- Generate matching order items (1–5 items per order, deterministic mapping)
WITH RECURSIVE
    nums(n) AS (SELECT 1
                UNION ALL
                SELECT n + 1
                FROM nums
                WHERE n < 100),
    orders AS (SELECT n AS OrderId,
                      ((n - 1) % 5) + 1 AS ItemCount
FROM nums
    ), idx(i) AS (
SELECT 1
UNION ALL
SELECT i + 1
FROM idx
WHERE i
    < 5
    )
    , order_items AS (
SELECT
    o.OrderId, ((o.OrderId + idx.i - 1) % 50) + 1 AS ProductId, ((o.OrderId * idx.i) % 20) + 1 AS Quantity
FROM orders o
    JOIN idx
ON idx.i <= o.ItemCount
    )
INSERT
INTO OrderItems(OrderId, ProductId, Quantity)
SELECT OrderId, ProductId, Quantity
FROM order_items;

COMMIT;
PRAGMA
foreign_keys = ON;
