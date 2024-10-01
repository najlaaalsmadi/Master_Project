document
  .getElementById("loginForm")
  .addEventListener("submit", function (event) {
    event.preventDefault(); // منع إرسال النموذج بشكل افتراضي
    debugger;
    // الحصول على بيانات الإدخال
    const email = document.getElementById("Email").value;
    const password = document.getElementById("Password").value;
    const userType = document.getElementById("UserType").value;

    // إرسال البيانات إلى API
    fetch("http://localhost:38146/api/Users/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        email: email,
        password: password,
        role: userType, // إرسال نوع المستخدم كـ "role"
      }),
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error("Network response was not ok");
        }
        return response.json();
      })
      .then((data) => {
        console.log(data); // Log the entire response data
        if (data.token) {
          // Save user ID and email to localStorage or sessionStorage
          localStorage.setItem("email", email); // تخزين البريد الإلكتروني

          Swal.fire({
            title: "نجاح!",
            text: `تم تسجيل الدخول بنجاح كـ ${
              userType === "user" ? "مستخدم" : "مدرب"
            }`,
            icon: "success",
            confirmButtonText: "موافق",
          }).then(() => {
            // إعادة توجيه المستخدم بناءً على نوعه
            window.location.href =
              userType === "user"
                ? "/frontend/user/Index.html"
                : "/frontend/user/Instructors.html";
          });
        } else {
          Swal.fire({
            title: "خطأ!",
            text: "البريد الإلكتروني أو كلمة المرور غير صحيحة",
            icon: "error",
            confirmButtonText: "حاول مجددًا",
          });
        }
      })
      .catch((error) => {
        console.error("There was a problem with the fetch operation:", error);
        Swal.fire({
          title: "خطأ!",
          text: "حدث خطأ أثناء الاتصال بالخادم",
          icon: "error",
          confirmButtonText: "حاول مجددًا",
        });
      });
  });
