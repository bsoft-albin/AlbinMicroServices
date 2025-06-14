-- name: UserRegisterQuery
INSERT INTO users (Username, Password, Email) VALUES (@username, @password, @email);SELECT LAST_INSERT_ID();

-- name: UserRoleAdd
INSERT INTO user_roles (UserId, Role) VALUES (@userId, @role);