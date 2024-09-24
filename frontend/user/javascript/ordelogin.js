document.addEventListener("DOMContentLoaded", function () {
  const authLinksContainer = document.getElementById("authLinks");
  const signupData = JSON.parse(localStorage.getItem("signupData"));

  if (signupData && signupData[2]) {
    authLinksContainer.innerHTML = `
<div class="icon-buttons d-flex align-items-center">

<!-- أيقونة الإشعارات -->
<a href="#" class="me-3" data-bs-toggle="dropdown" aria-expanded="false" data-bs-offset="0,9">
  <i class="fas fa-bell" style="color:white; font-size: 1rem;"></i>
  <span class="badge bg-danger">
    3
    <span class="visually-hidden">إشعارات جديدة</span>
  </span>
</a>

<!-- قائمة الإشعارات -->
<ul class="dropdown-menu shadow-lg border-0 rounded-3" style="width: 300px; background-color: #f9f9f9; margin-left:89px;">
  <li class="dropdown-header text-center fw-bold text-dark" style="background-color: #f1f1f1; border-bottom: 1px solid #ccc;">
    الإشعارات
  </li>
  <li><a href="#" class="dropdown-item border-bottom" style="color:black">إشعار 1: تم تحديث بيانات الحساب.</a></li>
  <li><a href="#" class="dropdown-item border-bottom" style="color:black">إشعار 2: يوجد عرض جديد.</a></li>
  <li><a href="#" class="dropdown-item border-bottom" style="color:black">إشعار 3: تم إضافة منتج جديد.</a></li>
  <li><a href="#" class="dropdown-item text-center py-3 text-primary">عرض جميع الإشعارات</a></li>
</ul>
<a href="/frontend/user/wishlist.html" class="me-3 d-flex align-items-center" style="text-decoration: none;">
  <i class="fas fa-heart" style="color:white; font-size: 1rem;"></i>
  <span class="badge bg-danger ms-1">2</span>
  </a>


  <!-- أيقونة عربة التسوق -->
  <a href="/frontend/user/card.html" class="me-3">
    <i class="fas fa-shopping-cart" style="color:white; font-size: 1rem;"></i>
    <span class="badge bg-danger">2</span>
  </a>

  <!-- قائمة المستخدم -->
  <div class="dropdown">
    <button class="dropdown-toggle" type="button" id="dropdownMenuButton" data-bs-toggle="dropdown" aria-expanded="false" style="background-color: transparent; border: none;">
      <i class="fas fa-user" style="color:white; font-size: 1rem;"></i> <!-- أيقونة المستخدم باللون الأبيض -->
    </button>
    <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="dropdownMenuButton">
      <li>
        <a href="/frontend/user/profile.html" class="dropdown-item text-dark">
          <i class="fas fa-user me-2"></i> الملف الشخصي
        </a>
      </li>
      <li>
        <a href="#" onclick="logout()" class="dropdown-item text-danger">
          <i class="fas fa-sign-out-alt me-2"></i> تسجيل الخروج
        </a>
      </li>
    
    </ul>
  </div>
</div>


     `;
  } else {
    authLinksContainer.innerHTML = `
   <div class="icon-buttons d-flex align-items-center justify-content-start">
  <!-- أيقونة عربة التسوق -->
<a href="/frontend/user/wishlist.html" class="me-3 d-flex align-items-center" style="text-decoration: none;">
  <i class="fas fa-heart" style="color:white; font-size: 1rem;"></i>
  <span class="badge bg-danger ms-1">2</span>
  </a>


<!-- أيقونة عربة التسوق -->
<a href="/frontend/user/card.html" class="me-3 d-flex align-items-center" style="text-decoration: none;">
    <i class="fas fa-shopping-cart" style="color:white; font-size: 1rem;"></i>
    <span class="badge bg-danger ms-1">2</span>
</a>

  <!-- زر تسجيل الدخول -->
  <a href="/frontend/user/login.html" class="btn btn-warning btn-sm">تسجيل دخول</a>

</div>



     `;
  }
});

function logout() {
  localStorage.removeItem("signupData");
  location.href = "/frontend/user/Index.html";
  location.reload(); // إعادة تحميل الصفحة لتحديث حالة تسجيل الدخول
}
// تحديث عدد التنبيهات
function updateNotificationCount(count) {
  $("#notificationCount").text(count);
  if (count > 0) {
    $("#notificationCount").show();
  } else {
    $("#notificationCount").hide();
  }
}

// إضافة تنبيه إلى القائمة
function addNotification(message) {
  $("#notificationList").append(`
    <a href="#" class="list-group-item list-group-item-action">
      ${message}
    </a>
  `);
  $(".no-notifications").hide(); // إخفاء رسالة "لا توجد إشعارات"
}

// على سبيل المثال، إضافة 3 تنبيهات بعد 3 ثوانٍ
setTimeout(function () {
  updateNotificationCount(3);
  addNotification("إشعار 1: تم تحديث بيانات الحساب.");
  addNotification("إشعار 2: يوجد عرض جديد.");
  addNotification("إشعار 3: تم إضافة منتج جديد.");
}, 3000);
