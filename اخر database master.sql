CREATE DATABASE project_Master_core;
USE project_Master_core;
-- 1. Users Table (مستخدمين)
CREATE TABLE Users (
    user_id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(100),
    email NVARCHAR(100) UNIQUE,
    password NVARCHAR(255),
    role NVARCHAR(20) CHECK (role IN ('user', 'trainer', 'admin')),
    created_at DATETIME DEFAULT GETDATE()
);

-- 2. Trainers Table (المدربين)
CREATE TABLE Trainers (
    trainer_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT,
    bio NVARCHAR(MAX),
    experience NVARCHAR(255),
    specialization NVARCHAR(255),
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- 3. Courses Table (المسارات التعليمية)
CREATE TABLE Courses (
    course_id INT PRIMARY KEY IDENTITY(1,1),  -- مفتاح رئيسي تلقائي
    title NVARCHAR(255),  -- عنوان الدورة
    description NVARCHAR(MAX),  -- وصف الدورة
    trainer_id INT,  -- معرف المدرب
    category_id INT,  -- معرف الفئة (مفتاح أجنبي مرتبط بفئة الدورة)
    price DECIMAL(10, 2),  -- سعر الدورة
    duration INT,  -- مدة الدورة (بالساعات أو الأيام)
    allowed_students INT,  -- عدد الطلاب المسموح به
    syllabus NVARCHAR(MAX),  -- المنهج الدراسي للدورة
    tools NVARCHAR(MAX),  -- الأدوات المطلوبة للدورة
    start_date DATE,  -- تاريخ بدء الدورة
    end_date DATE,  -- تاريخ انتهاء الدورة
    image_url NVARCHAR(255),  -- رابط صورة الدورة
    created_at DATETIME DEFAULT GETDATE(),  -- تاريخ إنشاء الدورة
    FOREIGN KEY (trainer_id) REFERENCES Trainers(trainer_id),  -- علاقة مع جدول المدربين
    FOREIGN KEY (category_id) REFERENCES Categories(category_id)  -- علاقة مع جدول الفئات
);
ALTER TABLE Courses
ADD Rating int DEFAULT 5;




-- 4. Lessons Table (الدروس)
CREATE TABLE Lessons (
    lesson_id INT PRIMARY KEY IDENTITY(1,1),
    course_id INT,
    title NVARCHAR(255),
    content NVARCHAR(MAX),
    video_url NVARCHAR(255),
    [order] INT,
    FOREIGN KEY (course_id) REFERENCES Courses(course_id)
);

-- 5. Handmade Products Table (المنتجات اليدوية)
CREATE TABLE Handmade_Products (
    product_id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255),
    description NVARCHAR(MAX),
    price DECIMAL(10, 2),
    stock INT,
    created_at DATETIME DEFAULT GETDATE()
);

-- 6. Learning Equipment Table (معدات التعلم)
CREATE TABLE Learning_Equipment (
    equipment_id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255),
    description NVARCHAR(MAX),
    price DECIMAL(10, 2),
    stock INT,
    created_at DATETIME DEFAULT GETDATE(),
    course_id INT,
    FOREIGN KEY (course_id) REFERENCES Courses(course_id)
);

-- 7. Orders Table (الطلبات)
CREATE TABLE Orders (
    order_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT,
    total_amount DECIMAL(10, 2),
    status NVARCHAR(20) CHECK (status IN ('pending', 'completed', 'cancelled')),
    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- 9. Events Table (الأحداث)
CREATE TABLE Events (
    event_id INT PRIMARY KEY IDENTITY(1,1),
    title NVARCHAR(255),
    description NVARCHAR(MAX),
    [date] DATE,
    location NVARCHAR(255)
);

-- 13. Newsletter Table (النشرة البريدية)
CREATE TABLE Newsletter (
    newsletter_id INT PRIMARY KEY IDENTITY(1,1),
    email NVARCHAR(100) UNIQUE,
    subscribed_at DATETIME DEFAULT GETDATE(),
    event_id INT,
    FOREIGN KEY (event_id) REFERENCES Events(event_id)
);

-- 14. Admins Table (المديرين)
CREATE TABLE Admins (
    admin_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT,
    access_level NVARCHAR(50),
    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- 15. Categories Table (التصنيفات)
CREATE TABLE Categories (
    category_id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) UNIQUE,
    description NVARCHAR(MAX)
);

-- 20. Comments Table (التعليقات)
CREATE TABLE Comments (
    comment_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT,
    item_type NVARCHAR(50),
    item_id INT,
    comment_text NVARCHAR(MAX),
    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

 
	use [project_Master_core];

INSERT INTO Trainers (user_id, bio, experience, specialization) VALUES 
(2, N'مدربة خبيرة في الفنون اليدوية.', N'10 سنوات', N'الفنون اليدوية'),
(2, N'مدرب متخصص في إصلاح الأجهزة التقنية.', N'8 سنوات', N'إصلاح الأجهزة');
INSERT INTO Users (name, email, password, role) VALUES 
(N'أحمد صالح', 'ahmed@example.com', 'hashedpassword1', 'user'),
(N'مروة عبد الله', 'marwa@example.com', 'hashedpassword2', 'trainer'),
(N'خالد محمود', 'khaled@example.com', 'hashedpassword3', 'admin');











INSERT INTO Courses (title, description, trainer_id, category_id, price, duration, allowed_students, syllabus, tools, start_date, end_date, image_url)
VALUES 
-- فئة الحرف اليدوية (Handicrafts)
(N'صناعة الفخار', N'تعلم أساسيات صناعة الفخار.', 1, 2, 100.00, 40, 15, N'أساسيات العمل بالطين', N'أدوات الطين والفرن', '2024-10-01', '2024-11-01', N'/images/pottery.jpg'),
(N'النسيج اليدوي', N'تقنيات النسيج اليدوي من الصفر.', 1, 2, 80.00, 30, 12, N'تقنيات النسيج اليدوي.', N'أدوات النسيج والخيوط', '2024-10-10', '2024-11-10', N'/images/weaving.jpg'),
(N'الحياكة والكروشيه', N'تعلم الحياكة الأساسية والمتقدمة.', 1, 2, 60.00, 25, 10, N'أنماط الحياكة والكروشيه.', N'إبر حياكة، خيوط', '2024-10-20', '2024-11-20', N'/images/knitting.jpg'),
(N'صنع المجوهرات اليدوية', N'تقنيات تصميم المجوهرات اليدوية.', 1, 2, 120.00, 30, 12, N'التصميم باستخدام المعادن والأحجار.', N'أدوات صناعة المجوهرات', '2024-11-01', '2024-12-01', N'/images/jewelry.jpg'),
(N'تشكيل الخشب', N'تعلم النحت على الخشب وصنع منتجات.', 1, 2, 150.00, 40, 15, N'تقنيات النحت اليدوي.', N'أدوات النحت', '2024-11-10', '2024-12-10', N'/images/woodworking.jpg'),
(N'العمل بالخيوط والتطريز', N'تقنيات الخياطة والتطريز اليدوي.', 1, 2, 50.00, 20, 8, N'أساسيات الخياطة.', N'إبر وخيوط', '2024-09-25', '2024-10-25', N'/images/embroidery.jpg'),
(N'صناعة المكرميه', N'تقنيات صنع الحبال والمكرمية.', 1, 2, 70.00, 25, 10, N'أساسيات المكرمية.', N'أدوات المكرمية', '2024-10-05', '2024-11-05', N'/images/macrame.jpg'),
(N'توزيعات المكرميه', N'ابتكار توزيعات المكرميه اليدوية.', 1, 2, 90.00, 30, 12, N'ابتكار تصاميم التوزيعات.', N'حبال وخيوط مكرمية', '2024-10-15', '2024-11-15', N'/images/macrame_decor.jpg'),
(N'صناعة الشمع', N'تعلم تقنيات صناعة الشموع.', 1, 2, 100.00, 35, 15, N'تصميم الشموع والألوان.', N'شموع وأدوات التشكيل', '2024-09-20', '2024-10-20', N'/images/candles.jpg'),
(N'صناعة الصابون', N'صنع الصابون الطبيعي بطرق آمنة.', 1, 2, 110.00, 30, 12, N'تركيب الصابون والعطور.', N'مواد خام لصناعة الصابون', '2024-09-15', '2024-10-15', N'/images/soap_making.jpg'),

-- فئة الفنون اليدوية (Handmade Arts)
(N'الرسم على الزجاج', N'تعلم تقنيات الرسم على الزجاج.', 1, 3, 75.00, 20, 10, N'الرسم باستخدام ألوان الزجاج.', N'ألوان زجاجية، فرشاة', '2024-09-25', '2024-10-25', N'/images/glass_painting.jpg'),
(N'تصميم الإكسسوارات', N'إبداع تصميم الإكسسوارات.', 1, 3, 90.00, 30, 12, N'تصميم باستخدام مواد مختلفة.', N'أدوات صنع الإكسسوارات', '2024-10-01', '2024-11-01', N'/images/accessory_design.jpg'),
(N'فن الورق (أوريغامي)', N'تعلم فن الأوريغامي.', 1, 3, 50.00, 15, 8, N'أساسيات طي الورق.', N'ورق خاص وأدوات طي', '2024-10-10', '2024-11-10', N'/images/origami.jpg'),
(N'فن السيراميك', N'تصميم السيراميك باستخدام تقنيات حديثة.', 1, 3, 120.00, 35, 15, N'صناعة قطع السيراميك.', N'أدوات تشكيل السيراميك', '2024-10-15', '2024-11-15', N'/images/ceramic_art.jpg'),
(N'التصميم باستخدام المواد المعاد تدويرها', N'ابتكار تصاميم باستخدام مواد معاد تدويرها.', 1, 3, 60.00, 20, 10, N'إعادة تدوير المواد في التصميم.', N'مواد معاد تدويرها', '2024-09-20', '2024-10-20', N'/images/upcycled_design.jpg'),

-- فئة الموضة والتنسيق (Fashion & Styling)
(N'تصميم الأزياء', N'أساسيات تصميم الأزياء من البداية إلى الاحتراف.', 1, 4, 200.00, 50, 20, N'تصميم الملابس وتنسيقها.', N'أقمشة وأدوات خياطة.', '2024-10-05', '2024-12-05', N'/images/fashion_design.jpg'),
(N'تنسيق الملابس', N'دورة لتنسيق الملابس حسب الأحداث.', 1, 4, 80.00, 20, 10, N'تنسيق الملابس والألوان.', N'أقمشة واكسسوارات.', '2024-09-30', '2024-10-30', N'/images/clothing_coordination.jpg'),

-- فئة إصلاح التكنولوجيا (Tech Repair)
(N'إصلاح الهواتف الذكية', N'دورة متقدمة في إصلاح الهواتف الذكية.', 1, 5, 150.00, 40, 15, N'تشخيص الأعطال وإصلاحها.', N'أدوات تصليح.', '2024-10-15', '2024-11-15', N'/images/smartphone_repair.jpg'),
(N'إصلاح أجهزة الكمبيوتر', N'تعلم إصلاح أعطال الكمبيوتر.', 1, 5, 130.00, 35, 12, N'تشخيص وإصلاح أعطال الأجهزة.', N'أدوات إصلاح أجهزة الكمبيوتر.', '2024-10-20', '2024-11-20', N'/images/computer_repair.jpg'),

-- فئة فن الطهي (Culinary Arts)
(N'تقنيات الطهي الأساسية', N'تعلم أساسيات الطهي المنزلي.', 2, 7, 90.00, 30, 12, N'إعداد وجبات أساسية.', N'مكونات الطهي الأساسية.', '2024-09-15', '2024-10-15', N'/images/basic_cooking.jpg'),
(N'فنون تزيين الكيك', N'دورة احترافية في تزيين الكيك.', 2, 7, 120.00, 35, 15, N'تقنيات تزيين الكيك المتقدمة.', N'أدوات تزيين الكيك.', '2024-10-01', '2024-11-01', N'/images/cake_decorating.jpg'),

-- فئة المنزل والحديقة (Home & Garden)
(N'تصميم الديكور الداخلي', N'أساسيات تصميم الديكور الداخلي.', 2, 8, 180.00, 45, 18, N'تنسيق الأثاث والألوان.', N'أدوات تصميم.', '2024-10-05', '2024-11-05', N'/images/interior_design.jpg'),
(N'زراعة النباتات والحدائق', N'تعلم زراعة النباتات وصيانة الحدائق.', 2, 8, 70.00, 20, 10, N'طرق العناية بالنباتات.', N'أدوات الزراعة.', '2024-09-25', '2024-10-25', N'/images/gardening.jpg');


INSERT INTO Courses (title, description, trainer_id, category_id, price, duration, allowed_students, syllabus, tools, start_date, end_date, image_url)
VALUES 
-- دورة الأعمال الخشبية (Woodworking)
(N'الأعمال الخشبية', N'تعلم كيفية تشكيل الخشب وصناعة منتجات خشبية.', 2, 2, 150.00, 40, 15, N'تقنيات النحت وتشكيل الخشب.', N'أدوات النحت على الخشب.', '2024-10-05', '2024-11-05', N'/images/woodworking.jpg'),

-- دورة صناعة المجوهرات (Handmade Jewelry)
(N'صناعة المجوهرات', N'تعلم تصميم وصنع المجوهرات اليدوية باستخدام مواد متنوعة.', 2, 2, 120.00, 30, 12, N'تصميم المجوهرات واستخدام المعادن والأحجار.', N'أدوات صنع المجوهرات.', '2024-10-15', '2024-11-15', N'/images/jewelry_making.jpg'),

-- دورة صناعة الصابون (Soap Making)
(N'صناعة الصابون', N'تعلم كيفية صنع الصابون الطبيعي واستخدام الزيوت العطرية.', 2, 2, 110.00, 35, 15, N'تركيب الصابون والعطور والزيوت.', N'مواد خام لصنع الصابون.', '2024-09-20', '2024-10-20', N'/images/soap_making.jpg'),

-- دورة الطباعة ثلاثية الأبعاد (3D Printing)
(N'الطباعة ثلاثية الأبعاد', N'تعلم تقنيات الطباعة ثلاثية الأبعاد وتصميم النماذج الرقمية.', 2, 5, 200.00, 50, 20, N'تصميم النماذج والطباعة باستخدام طابعات ثلاثية الأبعاد.', N'طابعات ثلاثية الأبعاد وأدوات تصميم.', '2024-11-01', '2024-12-01', N'/images/3d_printing.jpg'),

-- دورة صباغة الأقمشة (Fabric Dyeing)
(N'صباغة الأقمشة', N'تعلم تقنيات صباغة الأقمشة باستخدام الألوان الطبيعية والاصطناعية.', 2, 3, 90.00, 25, 10, N'تقنيات الصباغة والتلوين.', N'أدوات وألوان الصباغة.', '2024-09-25', '2024-10-25', N'/images/fabric_dyeing.jpg'),

-- دورة الأعمال الجلدية (Leathercraft)
(N'الأعمال الجلدية', N'تعلم تصميم وصنع المنتجات الجلدية اليدوية.', 2, 2, 130.00, 30, 12, N'تصميم المنتجات الجلدية والنقش على الجلود.', N'أدوات القص والنقش على الجلود.', '2024-10-10', '2024-11-10', N'/images/leathercraft.jpg'),

-- دورة أساسيات المكياج (Makeup Basics)
(N'أساسيات المكياج', N'تعلم تقنيات المكياج الأساسية وخطوات وضعه بشكل احترافي.', 2, 6, 80.00, 20, 10, N'أساسيات وضع المكياج للمبتدئين.', N'أدوات مكياج.', '2024-09-30', '2024-10-30', N'/images/makeup_basics.jpg'),

-- دورة إصلاح الأثاث (Furniture Repair)
(N'إصلاح الأثاث', N'تعلم تقنيات إصلاح وصيانة الأثاث المنزلي.', 2, 8, 140.00, 35, 12, N'إصلاح وترميم الأثاث باستخدام الأدوات المناسبة.', N'أدوات النجارة والصيانة.', '2024-10-20', '2024-11-20', N'/images/furniture_repair.jpg');

