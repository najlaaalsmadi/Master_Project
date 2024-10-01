// Request OTP Function
// Function to request OTP
async function requestOtp() {
  const formData = new FormData();
  formData.append("Email", document.getElementById("email").value);

  console.log("Sending OTP...");

  try {
    const response = await fetch(
      "http://localhost:38146/api/Users/forget-password",
      {
        method: "POST",
        body: formData,
      }
    );

    if (response.ok) {
      Swal.fire({
        icon: "success",
        title: "تم الإرسال",
        text: "تم إرسال OTP إلى بريدك الإلكتروني.",
        confirmButtonText: "حسناً",
      }).then(() => {
        // الانتقال إلى الخطوة الثانية
        showStep(2);
      });
    } else {
      Swal.fire({
        icon: "error",
        title: "خطأ",
        text: "فشل في إرسال OTP.",
        confirmButtonText: "حسناً",
      });
    }
  } catch (error) {
    console.error("Error:", error);
    Swal.fire({
      icon: "error",
      title: "خطأ",
      text: "حدث خطأ أثناء إرسال OTP.",
      confirmButtonText: "حسناً",
    });
  }
}

// Function to verify OTP and reset password
async function verifyOtp() {
  const otp = document.getElementById("otp").value;
  const newPassword = document.getElementById("newPasswordStep2").value;

  if (!otp || !newPassword) {
    Swal.fire({
      icon: "warning",
      title: "تنبيه",
      text: "يرجى إدخال OTP وكلمة المرور الجديدة.",
      confirmButtonText: "حسناً",
    });
    return;
  }

  const formData = new FormData();
  formData.append("Email", document.getElementById("email").value);
  formData.append("OTP", otp);
  formData.append("NewPassword", newPassword);
  console.log("OTP:", otp);
  console.log("NewPassword:", newPassword);
  console.log("Verifying OTP...");

  try {
    const response = await fetch(
      "http://localhost:38146/api/Users/verify-otp",
      {
        method: "POST",
        body: formData,
      }
    );
    console.log("response otp", response);

    if (response.ok) {
      Swal.fire({
        icon: "success",
        title: "تم التحقق",
        text: "تم تغيير كلمة المرور بنجاح.",
        confirmButtonText: "حسناً",
      }).then((result) => {
        if (result.isConfirmed) {
          // Redirect to the login page
          window.location.href = "/frontend/user/login.html"; // Update the path as necessary
        }
      });
    } else {
      Swal.fire({
        icon: "error",
        title: "خطأ",
        text: "OTP غير صحيح.",
        confirmButtonText: "حسناً",
      });
    }
  } catch (error) {
    console.error("Error:", error);
    Swal.fire({
      icon: "error",
      title: "خطأ",
      text: "حدث خطأ أثناء التحقق من OTP.",
      confirmButtonText: "حسناً",
    });
  }
}

// Function to show the current step of the process
function showStep(stepNumber) {
  // Hide all steps
  const steps = document.querySelectorAll(".step");
  steps.forEach((step) => {
    step.style.display = "none";
  });

  // Show the selected step
  document.getElementById(`step${stepNumber}`).style.display = "block";
}

// Initial call to show the first step
showStep(1);

// دالة للتحقق من البريد الإلكتروني (مثال)
function validateEmail(email) {
  const re = /\S+@\S+\.\S+/;
  return re.test(email);
}

// دالة للتحقق من إدخال البريد الإلكتروني قبل طلب OTP
function submitEmail() {
  const email = document.getElementById("email").value;

  if (!validateEmail(email)) {
    Swal.fire({
      icon: "warning",
      title: "تنبيه",
      text: "يرجى إدخال بريد إلكتروني صالح.",
      confirmButtonText: "حسناً",
    });
    return;
  }

  // طلب OTP بعد التحقق من صحة البريد الإلكتروني
  requestOtp();
}
