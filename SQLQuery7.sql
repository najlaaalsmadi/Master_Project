CREATE DATABASE project_Master;
USE project_Master;

-- 1. Users Table (مستخدمين)
CREATE TABLE Users (
    user_id INT PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(100),
    email VARCHAR(100) UNIQUE,
    password VARCHAR(255),
    role VARCHAR(20) CHECK (role IN ('user', 'trainer', 'admin')),
    created_at DATETIME DEFAULT GETDATE()
);

-- 2. Trainers Table (المدربين)
CREATE TABLE Trainers (
    trainer_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT,
    bio TEXT,
    experience VARCHAR(255),
    specialization VARCHAR(255),
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- 3. Courses Table (المسارات التعليمية)
CREATE TABLE Courses (
    course_id INT PRIMARY KEY IDENTITY(1,1),
    title VARCHAR(255),
    description TEXT,
    trainer_id INT,
    price DECIMAL(10, 2),
    duration INT,  -- Duration in hours or minutes
    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (trainer_id) REFERENCES Trainers(trainer_id)
);

-- 4. Lessons Table (الدروس)
CREATE TABLE Lessons (
    lesson_id INT PRIMARY KEY IDENTITY(1,1),
    course_id INT,
    title VARCHAR(255),
    content TEXT,
    video_url VARCHAR(255),
    [order] INT,  -- Order of lessons in the course
    FOREIGN KEY (course_id) REFERENCES Courses(course_id)
);

-- 5. Handmade Products Table (المنتجات اليدوية)
CREATE TABLE Handmade_Products (
    product_id INT PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(255),
    description TEXT,
    price DECIMAL(10, 2),
    stock INT,
    created_at DATETIME DEFAULT GETDATE()
);

-- 6. Learning Equipment Table (معدات التعلم)
CREATE TABLE Learning_Equipment (
    equipment_id INT PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(255),
    description TEXT,
    price DECIMAL(10, 2),
    stock INT,
    created_at DATETIME DEFAULT GETDATE(),
    course_id INT,  -- ربط جدول المعدات بالدورات
    FOREIGN KEY (course_id) REFERENCES Courses(course_id)
);

-- 7. Orders Table (الطلبات)
CREATE TABLE Orders (
    order_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT,
    total_amount DECIMAL(10, 2),
    status VARCHAR(20) CHECK (status IN ('pending', 'completed', 'cancelled')),
    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- 8. Order Items Table (عناصر الطلب)
CREATE TABLE Order_Items (
    order_item_id INT PRIMARY KEY IDENTITY(1,1),
    order_id INT,
    product_id INT,
    quantity INT,
    price DECIMAL(10, 2),
    FOREIGN KEY (order_id) REFERENCES Orders(order_id),
);

-- 9. Events Table (الأحداث)
CREATE TABLE Events (
    event_id INT PRIMARY KEY IDENTITY(1,1),
    title VARCHAR(255),
    description TEXT,
    [date] DATE,
    location VARCHAR(255)
);

-- 10. Payment Table (الدفع)
CREATE TABLE Payment (
    payment_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT,
    order_id INT,
    amount DECIMAL(10, 2),
    payment_method VARCHAR(50) CHECK (payment_method IN ('credit_card', 'paypal', 'bank_transfer')),
    payment_date DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
    FOREIGN KEY (order_id) REFERENCES Orders(order_id)
);

-- 11. Notifications Table (التنبيهات)
CREATE TABLE Notifications (
    notification_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT,
    content TEXT,
    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- 12. Communication Table (تواصل)
CREATE TABLE Communication (
    message_id INT PRIMARY KEY IDENTITY(1,1),
    sender_id INT,
    receiver_id INT,
    message_content TEXT,
    sent_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (sender_id) REFERENCES Users(user_id),
    FOREIGN KEY (receiver_id) REFERENCES Users(user_id)
);

-- 13. Newsletter Table (النشرة البريدية)
CREATE TABLE Newsletter (
    newsletter_id INT PRIMARY KEY IDENTITY(1,1),
    email VARCHAR(100) UNIQUE,
    subscribed_at DATETIME DEFAULT GETDATE(),
	    event_id INT,
    FOREIGN KEY (event_id) REFERENCES Events(event_id)

);

-- 14. Admins Table (المديرين)
CREATE TABLE Admins (
    admin_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT,
    access_level VARCHAR(50),  -- Defines the level of access (e.g., super admin, moderator)
    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- 15. Categories Table (التصنيفات)
CREATE TABLE Categories (
    category_id INT PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(255) UNIQUE,
    description TEXT
);

-- 16. Table linking Categories with Learning Equipment (جدول ربط التصنيفات بمعدات التعلم)
CREATE TABLE Category_Learning_Equipment (
    category_id INT,
    equipment_id INT,
    PRIMARY KEY (category_id, equipment_id),
    FOREIGN KEY (category_id) REFERENCES Categories(category_id),
    FOREIGN KEY (equipment_id) REFERENCES Learning_Equipment(equipment_id)
);

-- 17. Table linking Categories with Handmade Products (جدول ربط التصنيفات بالمنتجات اليدوية)
CREATE TABLE Category_Handmade_Products (
    category_id INT,
    product_id INT,
    PRIMARY KEY (category_id, product_id),
    FOREIGN KEY (category_id) REFERENCES Categories(category_id),
    FOREIGN KEY (product_id) REFERENCES Handmade_Products(product_id)
);

-- 18. Table linking Categories with Courses (جدول ربط التصنيفات بالدورات التعليمية)
CREATE TABLE Category_Courses (
    category_id INT,
    course_id INT,
    PRIMARY KEY (category_id, course_id),
    FOREIGN KEY (category_id) REFERENCES Categories(category_id),
    FOREIGN KEY (course_id) REFERENCES Courses(course_id)
);


