
-- 20. Comments Table (التعليقات)
CREATE TABLE Comments (
    comment_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT,  -- المعلق (مستخدم)
    item_type VARCHAR(50),  -- نوع العنصر (دورة، منتج يدوي، حدث)
    item_id INT,  -- معرف العنصر (الذي يتم التعليق عليه)
    comment_text TEXT,  -- نص التعليق
    created_at DATETIME DEFAULT GETDATE(),  -- تاريخ إضافة التعليق
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- التأكد من أن `item_type` يحتوي فقط على القيم المسموح بها
ALTER TABLE Comments
ADD CONSTRAINT CK_ItemType CHECK (item_type IN ('Course', 'Handmade_Product', 'Event'));