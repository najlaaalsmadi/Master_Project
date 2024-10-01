document
  .getElementById("signupForm")
  .addEventListener("submit", function (event) {
    event.preventDefault(); // Prevent the default form submission

    // Collect form values
    let firstName = document.getElementById("FirstName").value;
    let lastName = document.getElementById("LastName").value;
    let email = document.getElementById("Email").value;
    let password = document.getElementById("Password").value;
    let confirmPassword = document.getElementById("ConfirmPassword").value;

    // Validate password match
    if (password !== confirmPassword) {
      Swal.fire({
        title: "خطأ!",
        text: "كلمة المرور وتأكيد كلمة المرور غير متطابقتين.",
        icon: "error",
        confirmButtonText: "موافق",
      });
      return;
    }

    // Prepare full name
    let fullName = `${firstName} ${lastName}`;

    // Prepare FormData for submission
    let formData = new FormData();
    formData.append("Name", fullName);
    formData.append("Email", email);
    formData.append("Password", password);
    formData.append("Role", "user"); // Setting a default role

    // API call using Fetch
    fetch("http://localhost:38146/api/Users/signup", {
      method: "POST",
      body: formData,
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error("Network response was not ok");
        }
        return response.json();
      })
      .then((data) => {
        if (data.token) {
          Swal.fire({
            title: "نجاح!",
            text: "تم تسجيل الحساب بنجاح.",
            icon: "success",
            confirmButtonText: "موافق",
          }).then(() => {
            const cardId = generateCardId(); // توليد CardId
            // Save the userId in localStorage
            localStorage.setItem("email", email); // تخزين البريد الإلكتروني
            localStorage.setItem("cardId", cardId); // تخزين CardId

            // Redirect to the user index page
            window.location.href = "/frontend/user/Index.html";
          });
        } else {
          Swal.fire({
            title: "خطأ!",
            text: data.message || "حدث خطأ أثناء تسجيل الحساب.",
            icon: "error",
            confirmButtonText: "موافق",
          });
        }
      })
      .catch((error) => {
        console.error("Error:", error);
        Swal.fire({
          title: "خطأ!",
          text: "حدث خطأ أثناء الاتصال بالخادم.",
          icon: "error",
          confirmButtonText: "موافق",
        });
      });
  });
function generateCardId() {
  // توليد رقم عشوائي كـ CardId
  return Math.floor(Math.random() * 1000000); // مثال على توليد رقم عشوائي
}
// التحقق من وجود CardId عند بدء التطبيق
window.onload = function () {
  if (!localStorage.getItem("cardId")) {
    const cardId = generateCardId();
    localStorage.setItem("cardId", cardId);
  }
};
