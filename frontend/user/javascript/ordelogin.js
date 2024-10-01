document.addEventListener("DOMContentLoaded", function () {
  debugger;
  const authLinksContainer = document.getElementById("authLinks");
  const email = localStorage.getItem("email");

  console.log("Email from localStorage:", email); // تحقق من وجود الإيميل
  debugger;
  if (email) {
    authLinksContainer.innerHTML = `
      <div class="icon-buttons d-flex align-items-center">
        <!-- أيقونة الإشعارات -->
        <a href="#" class="me-3" data-bs-toggle="dropdown" aria-expanded="false" data-bs-offset="0,9">
          <i class="fas fa-bell" style="color:white; font-size: 1rem;"></i>
          <span class="badge bg-danger">3</span>
        </a>
        <!-- قائمة الإشعارات -->
        <ul class="dropdown-menu shadow-lg border-0 rounded-3" style="width: 300px; background-color: #f9f9f9; margin-left:89px;">
          <li class="dropdown-header text-center fw-bold text-dark" style="background-color: #f1f1f1; border-bottom: 1px solid #ccc;">الإشعارات</li>
          <li><a href="#" class="dropdown-item border-bottom" style="color:black">إشعار 1: تم تحديث بيانات الحساب.</a></li>
          <li><a href="#" class="dropdown-item border-bottom" style="color:black">إشعار 2: يوجد عرض جديد.</a></li>
          <li><a href="#" class="dropdown-item border-bottom" style="color:black">إشعار 3: تم إضافة منتج جديد.</a></li>
          <li><a href="#" class="dropdown-item text-center py-3 text-primary">عرض جميع الإشعارات</a></li>
        </ul>
        <!-- أيقونة عربة التسوق -->
        <a href="/frontend/user/wishlist.html" class="me-3 d-flex align-items-center" style="text-decoration: none;">
          <i class="fas fa-heart" style="color:white; font-size: 1rem;"></i>
          <span class="badge bg-danger ms-1">2</span>
        </a>
        <a href="/frontend/user/card.html" class="me-3">
          <i class="fas fa-shopping-cart" style="color:white; font-size: 1rem;"></i>
          <span class="badge bg-danger">2</span>
        </a>
        <!-- قائمة المستخدم -->
        <div class="dropdown">
          <button class="dropdown-toggle" type="button" id="dropdownMenuButton" data-bs-toggle="dropdown" aria-expanded="false" style="background-color: transparent; border: none;">
            <i class="fas fa-user" style="color:white; font-size: 1rem;"></i>
          </button>
          <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="dropdownMenuButton">
            <li><a href="/frontend/user/profile.html" class="dropdown-item text-dark"><i class="fas fa-user me-2"></i> الملف الشخصي</a></li>
            <li><a href="#" onclick="logout()" class="dropdown-item text-danger"><i class="fas fa-sign-out-alt me-2"></i> تسجيل الخروج</a></li>
          </ul>
        </div>
      </div>
    `;
  } else {
    authLinksContainer.innerHTML = `
      <div class="icon-buttons d-flex align-items-center justify-content-start">
        <a href="/frontend/user/wishlist.html" class="me-3 d-flex align-items-center" style="text-decoration: none;">
          <i class="fas fa-heart" style="color:white; font-size: 1rem;"></i>
          <span class="badge bg-danger ms-1">2</span>
        </a>
        <a href="/frontend/user/card.html" class="me-3 d-flex align-items-center" style="text-decoration: none;">
          <i class="fas fa-shopping-cart" style="color:white; font-size: 1rem;"></i>
          <span class="badge bg-danger ms-1">2</span>
        </a>
        <a href="/frontend/user/login.html" class="btn btn-warning btn-sm">تسجيل دخول</a>
      </div>
    `;
  }
});

function logout() {
  localStorage.removeItem("email");
  location.href = "/frontend/user/Index.html";
  location.reload(); // إعادة تحميل الصفحة لتحديث حالة تسجيل الدخول
}
