document.addEventListener("DOMContentLoaded", function () {
  // استرجاع course_id من localStorage
  const course_id = localStorage.getItem("course_id");

  if (course_id) {
    // استدعاء API باستخدام fetch لجلب بيانات الدورة
    fetch(`http://localhost:38146/api/Courses/${course_id}`)
      .then((response) => {
        console.log(response);
        // تحقق من نجاح الاستجابة
        if (!response.ok) {
          throw new Error("Network response was not ok");
        }
        return response.json();
      })
      .then((data) => {
        // تحديث عناصر الصفحة باستخدام getElementById
        console.log(data);
        document.getElementById("courseName").textContent = data.title;
        document.getElementById("courseName2").textContent = data.title;

        // صورة الدورة
        document.getElementById(
          "courseImage"
        ).src = `/backend/image/${data.imageUrl}`;
        document.getElementById("courseImage").alt = data.title;

        // تاريخ الدورة
        document.getElementById(
          "courseDate"
        ).textContent = `تاريخ الدورة: من ${data.startDate} إلى ${data.endDate}`;

        // عدد الطلاب
        document.getElementById(
          "studentCount"
        ).textContent = `عدد الطلاب المسموح به: ${data.allowedStudents} طالبًا`;

        // مدة الدورة
        document.getElementById(
          "courseDuration"
        ).textContent = `مدة الدورة: ${data.duration} ساعة`;

        // السعر
        document.getElementById(
          "coursePrice"
        ).textContent = `السعر: ${data.price} دولار`;

        // نظرة عامة (وصف الدورة)
        document.getElementById("courseOverview").textContent =
          data.description;

        // المنهج الدراسي
        let curriculumList = document.getElementById("curriculumList");
        curriculumList.innerHTML = ""; // تأكد من تفريغ القائمة قبل الإضافة
        data.syllabus.split(",").forEach((item) => {
          let listItem = document.createElement("li");
          listItem.textContent = item;
          curriculumList.appendChild(listItem);
        });

        // الأدوات المستخدمة
        let toolsList = document.getElementById("toolsList");
        toolsList.innerHTML = ""; // تأكد من تفريغ القائمة قبل الإضافة
        data.tools.split(",").forEach((tool) => {
          let listItem = document.createElement("li");
          listItem.textContent = tool;
          toolsList.appendChild(listItem);
        });

        // جلب اسم المدرس باستخدام TrainerId
        if (data.trainerId) {
          fetch(`http://localhost:38146/api/Trainer/${data.trainerId}`)
            .then((response) => {
              if (!response.ok) {
                throw new Error("Network response was not ok");
              }
              return response.json();
            })
            .then((trainerData) => {
              // تحديث اسم المدرس
              document.getElementById("instructorName").textContent =
                trainerData.name;
            })
            .catch((error) => {
              console.error("Error fetching trainer data:", error);
            });
        } else {
          document.getElementById("instructorName").textContent =
            "المدرس غير متاح";
        }
      })
      .catch((error) => {
        console.error("Error fetching course data:", error);
      });
  } else {
    console.error("No courseId found in localStorage.");
  }
});
document
  .getElementById("paypalButton")
  .addEventListener("click", async function () {
    const isLoggedIn = localStorage.getItem("email"); // افتراض وجود البريد الإلكتروني في الـ LocalStorage كدليل لتسجيل الدخول

    if (!isLoggedIn) {
      // توجيه المستخدم إلى صفحة تسجيل الدخول إذا لم يكن مسجلًا
      window.location.href = "/frontend/user/login.html"; // استبدل هذا بالرابط الصحيح لصفحة تسجيل الدخول
    } else {
      // التحقق من تفاصيل تسجيل الدخول والانتقال إلى صفحة الدفع
      window.location.href = "/frontend/user/payment.html"; // صفحة الدفع تشبه PayPal
    }
  });
