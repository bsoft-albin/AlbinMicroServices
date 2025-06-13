-- name: GetUserById
SELECT 
    u.Id,
    u.FirstName,
    u.LastName,
    u.Email,
    u.PhoneNumber,
    u.IsActive,
    u.CreatedDate,
    r.Name AS RoleName
FROM Users u
LEFT JOIN Roles r ON u.RoleId = r.Id
WHERE u.Id = @UserId 
  AND u.IsActive = 1;

-- name: GetActiveUsers
SELECT 
    u.Id,
    u.FirstName,
    u.LastName,
    u.Email,
    u.IsActive,
    u.CreatedDate,
    COUNT(o.Id) AS TotalOrders
FROM Users u
LEFT JOIN Orders o ON u.Id = o.UserId
WHERE u.IsActive = 1
GROUP BY u.Id, u.FirstName, u.LastName, u.Email, u.IsActive, u.CreatedDate
ORDER BY u.CreatedDate DESC;