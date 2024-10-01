CREATE DATABASE project_Master_core;
USE project_Master_core;
-- 1. Users Table (مستخدمين)
CREATE TABLE Users (
    user_id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(100),
    email NVARCHAR(100) UNIQUE,
    password NVARCHAR(255),
	PasswordHash NVARCHAR(255),
	PasswordSalt NVARCHAR(255),
    role NVARCHAR(20) CHECK (role IN ('user', 'trainer', 'admin')),
    created_at DATETIME DEFAULT GETDATE()
);
ALTER TABLE Users
ADD PasswordSalt NVARCHAR(255);

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
drop TABLE Handmade_Products;
-- 5. Handmade Products Table (المنتجات اليدوية)
CREATE TABLE Handmade_Products (
    product_id INT PRIMARY KEY IDENTITY(1,1),
    category_id INT,
	    user_id INT,

    name NVARCHAR(255),
    description NVARCHAR(MAX),
    price DECIMAL(10, 2),
    created_at DATETIME DEFAULT GETDATE(),
    image_url_1 NVARCHAR(255), -- الصورة الأولى
    image_url_2 NVARCHAR(255), -- الصورة الثانية
    image_url_3 NVARCHAR(255), -- الصورة الثالثة
    rating DECIMAL(2, 1),      -- التقييم (مثلاً من 5.0)
    FOREIGN KEY (category_id) REFERENCES Categories(category_id),  -- علاقة مع جدول الفئات
    FOREIGN KEY (user_id) REFERENCES Users(user_id)


);
drop table Learning_Equipment;
-- 6. Learning Equipment Table (معدات التعلم)
CREATE TABLE Learning_Equipment (
    equipment_id INT PRIMARY KEY IDENTITY(200,1),
    category_id INT,
    name NVARCHAR(255),
    description NVARCHAR(MAX),
    price DECIMAL(10, 2),
    created_at DATETIME DEFAULT GETDATE(),
    course_id INT,
    image_url_1 NVARCHAR(255), -- الصورة الأولى
    image_url_2 NVARCHAR(255), -- الصورة الثانية
    image_url_3 NVARCHAR(255), -- الصورة الثالثة
    rating DECIMAL(2, 1),      -- التقييم (مثلاً من 5.0)
    FOREIGN KEY (course_id) REFERENCES Courses(course_id),
    FOREIGN KEY (category_id) REFERENCES Categories(category_id)  -- علاقة مع جدول الفئات
);

drop table Learning_Equipment;
-- 7. Orders Table (الطلبات)
CREATE TABLE Orders (
    order_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT,
    total_amount DECIMAL(10, 2),
    status NVARCHAR(20) CHECK (status IN ('pending', 'completed', 'cancelled')),
    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);
drop TABLE Events;
drop TABLE Newsletter;
-- 9. Events Table (الأحداث)
CREATE TABLE Events (
    EventID INT PRIMARY KEY IDENTITY(1,1),
    EventTitle NVARCHAR(255),
    EventDate DATE,
    EventTime TIME,
    Location NVARCHAR(255),
    Participants INT,
    Speaker NVARCHAR(255),
    Summary NVARCHAR(MAX),
    Learnings NVARCHAR(MAX),
    Features NVARCHAR(MAX),
    SeatsAvailable INT,
    Topics INT,
    Exams INT,
    Articles INT,
    Certificates INT,
    ImagePath NVARCHAR(255),
    MapURL NVARCHAR(MAX),
	ZoomLink NVARCHAR(255),
    ZoomPassword NVARCHAR(50)
);
CREATE TABLE Bookings (
    booking_id INT PRIMARY KEY IDENTITY(1,1), -- معرف فريد لكل حجز
    student_id INT, -- معرف الطالب الذي قام بالحجز (يرتبط بجدول الطلاب)
    EventID INT, -- معرف الفعالية التي تم الحجز لها (يرتبط بجدول الفعاليات)
    booking_date DATE DEFAULT GETDATE(), -- تاريخ الحجز (افتراضي هو تاريخ اليوم)
    status NVARCHAR(50) DEFAULT N'مؤكد', -- حالة الحجز (مثلاً: "مؤكد"، "ملغى")
    FOREIGN KEY (student_id) REFERENCES users(user_id), -- مفتاح أجنبي يربط الطالب بجدول المستخدمين
    FOREIGN KEY (EventID) REFERENCES Events(EventID) -- مفتاح أجنبي يربط الحجز بجدول الفعاليات
);


-- 13. Newsletter Table (النشرة البريدية)
CREATE TABLE Newsletter (
    newsletter_id INT PRIMARY KEY IDENTITY(1,1),
    email NVARCHAR(100) UNIQUE,
    subscribed_at DATETIME DEFAULT GETDATE(),
    EventID INT,
    FOREIGN KEY (EventID) REFERENCES Events(EventID)
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

-- معدات الحياكة الكاملة
INSERT INTO Learning_Equipment (category_id, name, description, price, rating, image_url_1, image_url_2, image_url_3)
VALUES (1, N'معدات الحياكة الكاملة', N'آلات الخياطة، الإبر والخيوط، الأقمشة، المقصات، أدوات القياس، وأنماط الخياطة. معدات خياطة كاملة.', 13.00, 5.0, NULL, NULL, NULL);

-- ماكينة الحياكة
INSERT INTO Learning_Equipment (category_id, name, description, price, rating, image_url_1, image_url_2, image_url_3)
VALUES (2, N'ماكينة الحياكة', N'معدات خياطة كاملة.', 150.00, 5.0, NULL, NULL, NULL);

-- حبات الكريستال
INSERT INTO Learning_Equipment (category_id, name, description, price, rating, image_url_1, image_url_2, image_url_3)
VALUES (3, N'حبات الكريستال', N'حبات الكريستال بأحجام وأسعار مختلفة.', 5.00, 5.0, NULL, NULL, NULL);

-- الأسلاك النحاسية
INSERT INTO Learning_Equipment (category_id, name, description, price, rating, image_url_1, image_url_2, image_url_3)
VALUES (4, N'الأسلاك النحاسية', N'أسلاك نحاسية ذات جودة عالية.', 15.00, 4.0, NULL, NULL, NULL);

-- شرائط وشريط قياس ومقص
INSERT INTO Learning_Equipment (category_id, name, description, price, rating, image_url_1, image_url_2, image_url_3)
VALUES (5, N'شرائط وشريط قياس ومقص', N'خيوط صوف ومعدات الحياكة.', 20.00, 4.0, NULL, NULL, NULL);

-- باترون
INSERT INTO Learning_Equipment (category_id, name, description, price, rating, image_url_1, image_url_2, image_url_3)
VALUES (6, N'باترون', N'باترون بأشكال متعددة.', 10.00, 4.0, NULL, NULL, NULL);

-- خيوط ماكينة ومقص خياطة وماسورة قياس
INSERT INTO Learning_Equipment (category_id, name, description, price, rating, image_url_1, image_url_2, image_url_3)
VALUES (7, N'خيوط ماكينة ومقص خياطة وماسورة قياس', N'معدات خياطة عالية الجودة.', 50.00, 5.0, NULL, NULL, NULL);

-- معدات صناعة ريزين قوالب وألوان
INSERT INTO Learning_Equipment (category_id, name, description, price, rating, image_url_1, image_url_2, image_url_3)
VALUES (8, N'معدات صناعة ريزين قوالب وألوان', N'معدات متكاملة لصناعة الريزين.', 30.00, 4.0, NULL, NULL, NULL);

-- معدات فنون الماكياج
INSERT INTO Learning_Equipment (category_id, name, description, price, rating, image_url_1, image_url_2, image_url_3)
VALUES (9, N'معدات فنون الماكياج', N'أدوات ماكياج احترافية.', 80.00, 5.0, NULL, NULL, NULL);


DROP TABLE Handmade_Products;

INSERT INTO Handmade_Products (category_id, user_id, name, description, price, image_url_1, rating)
VALUES (2, 1, N'شمع يدوي', N'منتج يدوي مصنوع من الشمع الطبيعي.', 150.00, N'شمع.jfif', 5.0);

INSERT INTO Handmade_Products (category_id, user_id, name, description, price, image_url_1, rating)
VALUES (2, 1, N'طاقية', N'طاقية يدوي مصنوع من القماش الفاخر.', 150.00, N'طاقية.jpg', 5.0);

INSERT INTO Handmade_Products (category_id, user_id, name, description, price, image_url_1, rating)
VALUES (2, 1, N'شماغ مهدب يدوي', N'شماغ مهدب يدوي مصنوع بجودة عالية.', 5.00, N'شماغ مهدب.jpg', 5.0);

INSERT INTO Handmade_Products (category_id, user_id, name, description, price, image_url_1, rating)
VALUES (3, 1, N'عرجة', N'عرجة يدوية.', 15.00, N'عرجه.jpg', 4.0);

INSERT INTO Handmade_Products (category_id, user_id, name, description, price, image_url_1, rating)
VALUES (4, 1, N'فستان كروشة', N'فستان كروشة يدوي.', 20.00, N'فستان ثاني كروشه.jfif', 4.0);

INSERT INTO Handmade_Products (category_id, user_id, name, description, price, image_url_1, rating)
VALUES (4, 1, N'فستان كروشة', N'فستان كروشة يدوي.', 10.00, N'فستان كروشه.jfif', 4.0);

INSERT INTO Handmade_Products (category_id, user_id, name, description, price, image_url_1, rating)
VALUES (3, 1, N'مكرمية', N'مكرمية يدوية.', 50.00, N'مكرميه.jfif', 5.0);

INSERT INTO Handmade_Products (category_id, user_id, name, description, price, image_url_1, rating)
VALUES (3, 1, N'اواني فخار', N'أواني فخارية يدوية.', 30.00, N'اواني فخار.jpg', 4.0);

INSERT INTO Handmade_Products (category_id, user_id, name, description, price, image_url_1, rating)
VALUES (4, 1, N'طقم اطفال', N'طقم أطفال يدوي.', 5.00, N'طقم طفل.jfif', 5.0);

INSERT INTO Handmade_Products (category_id, user_id, name, description, price, image_url_1, rating)
VALUES (4, 1, N'طقم الرحل', N'طقم الرحل يدوي.', 5.00, N'طقم الرحل.jfif', 5.0);

INSERT INTO Handmade_Products (category_id, user_id, name, description, price, image_url_1, rating)
VALUES (2, 1, N'فوانيس يدوية', N'فوانيس يدوية مزخرفة.', 5.00, N'فوانيس.jfif', 5.0);
drop table Comments_Courses_before;
CREATE TABLE Responses_Comments_Courses_before(
    response_id INT PRIMARY KEY IDENTITY(1,1), -- رقم الرد (المفتاح الأساسي)
    comment_id INT,                            -- رقم التعليق الذي يتم الرد عليه (مفتاح أجنبي)
    response_text NVARCHAR(MAX),               -- نص الرد
    created_at DATETIME DEFAULT GETDATE(),     -- وقت إنشاء الرد
    FOREIGN KEY (comment_id) REFERENCES Comments_Courses_before(comment_id) -- ربط الرد بالتعليق
);
CREATE TABLE Comments_Courses_before ( 
    comment_id INT PRIMARY KEY IDENTITY(1,1), -- رقم التعليق (المفتاح الأساسي)
    comment_text NVARCHAR(MAX),                -- نص التعليق
    created_at DATETIME DEFAULT GETDATE()      -- وقت إنشاء التعليق
);
ALTER TABLE Responses_Comments_Courses_before
ADD Name NVARCHAR(255);

CREATE TABLE Payments (
    PaymentId INT PRIMARY KEY IDENTITY,
    UserId INT, -- ربط بالمستخدم
    PaymentMethod NVARCHAR(50),
    Amount DECIMAL(18, 2),
    PaymentDate DATETIME,
    PayerId NVARCHAR(100),
    Status NVARCHAR(50),
    FOREIGN KEY (UserId) REFERENCES Users(user_id) -- فرضية أن لديك جدول Users
);

-- جدول لتخزين معلومات الحجوزات لكل فعالية.
-- يحتوي على تفاصيل حول الطالب الذي حجز فعالية معينة وتاريخ الحجز وحالة الحجز.
-- جدول لتخزين معلومات الفعاليات.
-- يحتوي على تفاصيل مثل اسم الفعالية، تاريخ البدء، عدد المواضيع والاختبارات والمقالات والشهادات، بالإضافة إلى السعة والموقع.

ALTER TABLE Users ADD otp NVARCHAR(max); 
ALTER TABLE Users ADD biographicaldetailsCV NVARCHAR(max); 
ALTER TABLE Users ADD phone NVARCHAR(10); 
ALTER TABLE Users ADD  imageprofile NVARCHAR(max); 

CREATE TABLE user_course (
    enrollment_id INT PRIMARY KEY IDENTITY(1,1),  -- Primary key for the join table
    user_id INT,  -- Foreign key to Users table
    course_id INT,  -- Foreign key to Courses table
    enrollment_date DATETIME DEFAULT GETDATE(),  -- Date when the user enrolled in the course
    progress INT DEFAULT 0,  -- User's progress in the course (optional field)
    completed BIT DEFAULT 0,  -- Indicates if the course is completed (optional field)
    FOREIGN KEY (user_id) REFERENCES Users(user_id),  -- Foreign key constraint
    FOREIGN KEY (course_id) REFERENCES Courses(course_id)  -- Foreign key constraint
);



-- Event 1
INSERT INTO Events (
    EventTitle, EventDate, EventTime, Location, Participants, Speaker, Summary, Learnings, Features, SeatsAvailable, Topics, Exams, Articles, Certificates, ImagePath, MapURL, ZoomLink, ZoomPassword
) 
VALUES (
    N'ورشة عمل: فنون الخياطة الأساسية والتقنيات المتقدمة', 
    '2024-10-05', 
    '10:00', 
    N'عمان، الأردن', 
    200, 
    N'أحمد الخياط', 
    N'ورشة عمل لتعليم فنون الخياطة الأساسية والتقنيات المتقدمة.', 
    N'أساسيات الخياطة، تقنيات متقدمة، تصميم الملابس.', 
    N'إتقان الخياطة بدون أخطاء، اختصارات عملية، أفكار إبداعية في التصميم.', 
    50, 
    5, 
    2, 
    10, 
    5, 
    N'ندوة فنون الخياطة الأساسية والتقنيات المتقدمة مع محترفي الصناعة.png', 
    'https://maps.app.goo.gl/AFBfBeLMq3BTZD4R7',
    'https://meet.google.com/tdu-fofq-mcv?authuser=0', 
    'password123'
);

-- Event 2
INSERT INTO Events (
    EventTitle, EventDate, EventTime, Location, Participants, Speaker, Summary, Learnings, Features, SeatsAvailable, Topics, Exams, Articles, Certificates, ImagePath, MapURL, ZoomLink, ZoomPassword
) 
VALUES (
    N'دورة تدريبية: أساسيات النجارة وبناء الأثاث', 
    '2024-10-12', 
    '12:00', 
    N'عمان، الأردن', 
    150, 
    N'محمد النجار', 
    N'دورة تدريبية شاملة لتعلم أساسيات النجارة وبناء الأثاث.', 
    N'أساسيات النجارة، بناء الأثاث، اختيار المواد.', 
    N'تقنيات عملية في بناء الأثاث، كيفية استخدام الأدوات بشكل فعال.', 
    40, 
    6, 
    2, 
    8, 
    4, 
    N'دورة تدريبية أساسيات النجارة وبناء الأثاث.png', 
    'https://maps.app.goo.gl/AFBfBeLMq3BTZD4R7',
    'https://meet.google.com/tdu-fofq-mcv?authuser=0', 
    'woodcraft123'
);

-- Event 3
INSERT INTO Events (
    EventTitle, EventDate, EventTime, Location, Participants, Speaker, Summary, Learnings, Features, SeatsAvailable, Topics, Exams, Articles, Certificates, ImagePath, MapURL, ZoomLink, ZoomPassword
) 
VALUES (
    N'ورشة عمل: تقنيات التجميل والعناية بالبشرة', 
    '2024-10-20', 
    '14:00', 
    N'عمان، الأردن', 
    180, 
    N'سارة التجميلية', 
    N'ورشة عمل لتعلم تقنيات التجميل الحديثة والعناية بالبشرة.', 
    N'العناية بالبشرة، تقنيات التجميل، المنتجات الطبيعية.', 
    N'كيفية العناية بأنواع البشرة المختلفة، طرق استخدام المواد الطبيعية.', 
    60, 
    4, 
    1, 
    6, 
    3, 
    N'ورشة عمل تقنيات التجميل والعناية بالبشرة.png', 
    'https://maps.app.goo.gl/AFBfBeLMq3BTZD4R7',
    'https://meet.google.com/tdu-fofq-mcv?authuser=0', 
    'beautycare123'
);

-- Event 4
INSERT INTO Events (
    EventTitle, EventDate, EventTime, Location, Participants, Speaker, Summary, Learnings, Features, SeatsAvailable, Topics, Exams, Articles, Certificates, ImagePath, MapURL, ZoomLink, ZoomPassword
) 
VALUES (
    N'دورة تعليمية: صناعة المجوهرات من الخرز', 
    '2024-10-28', 
    '16:00', 
    N'عمان، الأردن', 
    100, 
    N'ليلى الحرفية', 
    N'دورة تعليمية لصناعة المجوهرات باستخدام الخرز.', 
    N'كيفية صنع المجوهرات من الخرز، تقنيات الخرز.', 
    N'تصميمات مبتكرة لصنع المجوهرات باستخدام الخرز.', 
    35, 
    3, 
    1, 
    5, 
    2, 
    N'دورة تعليمية صناعة المجوهرات من الخرز.png', 
    'https://maps.app.goo.gl/AFBfBeLMq3BTZD4R7',
    'https://meet.google.com/tdu-fofq-mcv?authuser=0', 
    'jewelrycraft123'
);

-- Event 5
INSERT INTO Events (
    EventTitle, EventDate, EventTime, Location, Participants, Speaker, Summary, Learnings, Features, SeatsAvailable, Topics, Exams, Articles, Certificates, ImagePath, MapURL, ZoomLink, ZoomPassword
) 
VALUES (
    N'ورشة عمل: ورشة العمل المخصصة لشهر رمضان المبارك', 
    '2024-11-05', 
    '11:00', 
    N'عمان، الأردن', 
    300, 
    N'خالد الصانع', 
    N'ورشة عمل خاصة لتعليم فنون الخياطة والتطريز لرمضان.', 
    N'تطريز الملابس التقليدية، تصميمات إبداعية لشهر رمضان.', 
    N'تعلم مهارات تقليدية مع لمسة عصرية.', 
    80, 
    4, 
    3, 
    7, 
    3, 
    N'ورشة عمل فنون الخياطة لشهر رمضان.png', 
    'https://maps.app.goo.gl/AFBfBeLMq3BTZD4R7',
    'https://meet.google.com/tdu-fofq-mcv?authuser=0', 
    'ramadan123'
);

-- Event 6
INSERT INTO Events (
    EventTitle, EventDate, EventTime, Location, Participants, Speaker, Summary, Learnings, Features, SeatsAvailable, Topics, Exams, Articles, Certificates, ImagePath, MapURL, ZoomLink, ZoomPassword
) 
VALUES (
    N'برنامج تدريبي المنفذ في مركز زها الثقافي لتعليم مهنة الخياطة', 
    '2024-11-12', 
    '10:00', 
    N'عمان، الأردن', 
    250, 
    N'محمود الخياط', 
    N'برنامج تدريبي لتعليم الخياطة في مركز زها الثقافي.', 
    N'تقنيات الخياطة، اختيار الأقمشة، التفصيل.', 
    N'إتقان فنون الخياطة الأساسية والمتقدمة.', 
    70, 
    6, 
    2, 
    9, 
    4, 
    N'برنامج تدريبي الخياطة مركز زها.png', 
    'https://maps.app.goo.gl/AFBfBeLMq3BTZD4R7',
    'https://meet.google.com/tdu-fofq-mcv?authuser=0', 
    'zahacenter123'
);

-- Event 7
INSERT INTO Events (
    EventTitle, EventDate, EventTime, Location, Participants, Speaker, Summary, Learnings, Features, SeatsAvailable, Topics, Exams, Articles, Certificates, ImagePath, MapURL, ZoomLink, ZoomPassword
) 
VALUES (
    N'دورة متقدمة: تقنيات الفخار المتطورة في مركز زها الثقافي', 
    '2024-11-19', 
    '14:00', 
    N'عمان، الأردن', 
    180, 
    N'سامي الفخاري', 
    N'دورة متقدمة لتعلم أحدث تقنيات الفخار.', 
    N'أساسيات الفخار، تصميم الفخار المتقدم.', 
    N'تقنيات جديدة في صناعة الفخار، إتقان التفاصيل الدقيقة.', 
    50, 
    5, 
    2, 
    8, 
    3, 
    N'دورة تقنيات الفخار مركز زها.png', 
    'https://maps.app.goo.gl/AFBfBeLMq3BTZD4R7',
    'https://meet.google.com/tdu-fofq-mcv?authuser=0', 
    'pottery123'
);

-- Event 8
INSERT INTO Events (
    EventTitle, EventDate, EventTime, Location, Participants, Speaker, Summary, Learnings, Features, SeatsAvailable, Topics, Exams, Articles, Certificates, ImagePath, MapURL, ZoomLink, ZoomPassword
) 
VALUES (
    N'ورشة عمل متقدمة: أحدث صيحات التجميل والعناية الشخصية', 
    '2024-11-25', 
    '16:00', 
    N'عمان، الأردن', 
    150, 
    N'رنا خبيرة التجميل', 
    N'ورشة عمل لمناقشة أحدث تقنيات التجميل والعناية الشخصية.', 
    N'تسريحات الشعر، استخدام أدوات التجميل، العناية بالبشرة.', 
    N'أحدث تقنيات العناية بالبشرة والشعر، نصائح للعناية الشخصية.', 
    60, 
    4, 
    1, 
    7, 
    2, 
    N'ورشة عمل متقدمة التجميل.png', 
    'https://maps.app.goo.gl/AFBfBeLMq3BTZD4R7',
    'https://meet.google.com/tdu-fofq-mcv?authuser=0', 
    'beautytrends123'
);
drop TABLE Card;
drop TABLE CardItem;
-- إنشاء جدول Card
CREATE TABLE Card (
    CardId INT PRIMARY KEY,  -- المعرف الفريد للسلة
    UserId INT,                            -- معرف المستخدم
    CreatedAt DATETIME DEFAULT GETDATE(),  -- تاريخ إنشاء السلة
    UpdatedAt DATETIME DEFAULT GETDATE(),  -- تاريخ آخر تحديث للسلة
    FOREIGN KEY (UserId) REFERENCES Users(user_id) -- ربط السلة بالمستخدمين
);


drop table CardItem;
-- إنشاء جدول CardItem
CREATE TABLE CardItem (
    CardItemId INT IDENTITY(1,1) PRIMARY KEY,  -- المعرف الفريد للعنصر
    CardId INT,                                 -- معرف السلة المرتبط
    product_id INT,
	equipment_id INT,-- معرف المنتج
    Quantity INT DEFAULT 1,                     -- كمية المنتج المضاف
    Price DECIMAL(10, 2),                       -- سعر المنتج وقت الإضافة
    AddedAt DATETIME DEFAULT GETDATE(),         -- تاريخ إضافة العنصر للسلة
    FOREIGN KEY (CardId) REFERENCES Card(CardId) ON DELETE CASCADE,  -- ربط العنصر بالسلة وحذف العناصر عند حذف السلة
    FOREIGN KEY (product_id) REFERENCES Handmade_Products(product_id),
	FOREIGN KEY (equipment_id) REFERENCES Learning_Equipment(equipment_id)
);

