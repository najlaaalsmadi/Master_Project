// addcategory.js
document
  .getElementById("addCategoryForm")
  .addEventListener("submit", async (event) => {
    event.preventDefault(); // منع إرسال النموذج الافتراضي
    debugger;
    const newCategory = {
      name: document.getElementById("categoryName").value,
      description: document.getElementById("categoryDescription").value,
    };

    try {
      const response = await fetch(
        "http://localhost:38146/api/Category/Categories",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(newCategory),
        }
      );

      if (response.ok) {
        // عرض رسالة النجاح باستخدام SweetAlert
        await Swal.fire({
          icon: "success",
          title: "تم إضافة الفئة بنجاح",
          confirmButtonText: "موافق",
        }).then(() => {
          // إعادة التوجيه إلى صفحة عرض الفئات بعد إغلاق التنبيه
          window.location.href = "viewcategory.html"; // تحديث المسار إذا لزم الأمر
        });
      } else {
        await Swal.fire({
          icon: "error",
          title: "حدث خطأ أثناء إضافة الفئة",
          confirmButtonText: "موافق",
        });
      }
    } catch (error) {
      console.error("خطأ في إضافة الفئة:", error);
      await Swal.fire({
        icon: "error",
        title: "حدث خطأ أثناء إضافة الفئة",
        confirmButtonText: "موافق",
      });
    }
  });
