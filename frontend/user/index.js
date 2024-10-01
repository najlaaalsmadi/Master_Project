// استدعاء بيانات المنتجات ذات الصلة من API
fetch("http://localhost:38146/api/LearningEquipment/random")
  .then((response) => response.json())
  .then((data) => {
    const relatedProductsContainer = document.getElementById(
      "relatedProductsrandom"
    );

    // إنشاء العناصر لكل منتج
    data.forEach((product) => {
      const productHTML = `
 <div class="col-md-4 mb-4">
  <a href="javascript:void(0);" onclick="goToDetails3(${product.equipmentId})" class="card text-decoration-none text-dark">
     <img src="/backend/image/${product.imageUrl1}" class="card-img-top img-fluid" alt="${product.name}" />
     <div class="card-body text-center">
       <h5 class="card-title">${product.name}</h5>
       <p class="card-text">${product.price}$</p>
       <p class="card-text">⭐⭐⭐⭐⭐ (${product.rating})</p>
     </div>
   </a>
 </div>
`;
      // إضافة المنتج إلى الصفحة
      relatedProductsContainer.innerHTML += productHTML;
    });
  })
  .catch((error) => {
    console.error("Error fetching related products:", error);
  });

function goToDetails3(equipmentId) {
  localStorage.setItem("equipmentId1", equipmentId);
  window.location.href = `/frontend/user/equipmentshopdetails.html?id=${equipmentId}`;
}

fetch("http://localhost:38146/api/Handmade_Products/random")
  .then((response) => response.json())
  .then((data) => {
    const relatedProductsContainer = document.getElementById("Randomproduct");

    // إنشاء العناصر لكل منتج
    data.forEach((product) => {
      const productHTML = `
<div class="col-md-4 mb-4">
 <a href="javascript:void(0);" onclick="goToDetails21(${product.productId})" class="card text-decoration-none text-dark">
    <img src="/backend/image/${product.imageUrl1}" class="card-img-top img-fluid" alt="${product.name}" />
    <div class="card-body text-center">
      <h5 class="card-title">${product.name}</h5>
      <p class="card-text">${product.price}$</p>
      <p class="card-text">⭐⭐⭐⭐⭐ (${product.rating})</p>
    </div>
  </a>
</div>
`;
      // إضافة المنتج إلى الصفحة
      relatedProductsContainer.innerHTML += productHTML;
    });
  })
  .catch((error) => {
    console.error("Error fetching related products:", error);
  });

function goToDetails21(productId) {
  localStorage.setItem("productId", productId);
  window.location.href = `/frontend/user/HandmadeShopdetils.html?id=${productId}`;
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// جلب الأحداث من API
fetch("http://localhost:38146/api/Event/randomEvent")
  .then((response) => response.json())
  .then((events) => {
    const eventsContainer = document.getElementById("events-container-index");

    // تكرار الأحداث وإنشاء كروت HTML ديناميكيًا
    events.forEach((event) => {
      // إنشاء كرت لكل حدث
      const eventCard = `
  <div class="col-md-3 col-sm-6 event-card">
    <a
      href="javascript:void(0);" 
      onclick="goToEventDetails(${event.eventId})"
      class="text-decoration-none text-dark event-link"
    >
      <div class="card">
        <img
          src="/backend/image/${
            event.imagePath ? event.imagePath : "default.png"
          }"
          class="card-img-top event-img"
          alt="${event.eventTitle}"
        />
        <div class="card-body">
          <div class="event-date">${new Date(
            event.eventDate
          ).toLocaleDateString("ar-SA", {
            day: "numeric",
            month: "long",
            year: "numeric",
          })}</div>
          <div class="event-title">${event.eventTitle}</div>
        </div>
      </div>
    </a>
  </div>
`;

      // إضافة الكرت إلى الـ Container
      eventsContainer.innerHTML += eventCard;
    });
  })
  .catch((error) => {
    console.error("Error fetching events:", error);
  });

// الدالة لتخزين EventID وإعادة التوجيه
function goToEventDetails(eventId) {
  // تخزين EventID في localStorage
  localStorage.setItem("EventID", eventId);

  // إعادة التوجيه إلى صفحة التفاصيل مع تمرير EventID في رابط الـ URL
  window.location.href = `/frontend/user/EventsDetails.html?id=${eventId}`;
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Function to load all courses
function loadCourses() {
  fetch("http://localhost:38146/api/Courses/random")
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok " + response.statusText);
      }
      return response.json();
    })
    .then((data) => {
      const coursesContainer = document.getElementById(
        "courses-container-random"
      );
      coursesContainer.innerHTML = ""; // Clear the container before adding new content

      // Create course cards
      data.forEach((course) => {
        const courseCard = `
          <div class="col-md-4 mb-4">
            <div class="card h-100">
              <img src="/backend/image/${course.imageUrl}" class="card-img-top" alt="${course.title}" />
              <div class="card-body">
                <h5 class="card-title">${course.title}</h5>
                <p class="card-text">${course.description}</p>
                <p class="card-text">${course.price}دينار</p>
              </div>
              <button class="btn btn-light border-0 favorite-btn" style="background-color: transparent">
                <i class="fa fa-heart" style="color: #ff6b6b; font-size: 1.5em"></i>
              </button>
              <div class="card-footer btn btn-warning" style="background-color: orange">
              <a href="javascript:void(0);" onclick="goToDetails(${course.courseId})" style="text-decoration: none">
                  <strong class="text-orange" style="color: white">اذهب الى التفاصيل</strong>
                </a>
              </div>
              </div>
            </div>
          </div>
        `;
        coursesContainer.innerHTML += courseCard;
      });
    })
    .catch((error) => console.error("Error fetching courses:", error));
}

// Function to go to the course details page
function goToDetails(courseId) {
  localStorage.setItem("course_id", courseId);
  window.location.href = `/frontend/user/CourseDetails.html?id=${courseId}`;
}

// Load all courses when the page is opened
window.onload = loadCourses;

document.addEventListener("DOMContentLoaded", function () {
  // إضافة حدث على تغيّر مربعات التحديد للتقييمات
  document.querySelectorAll('input[type="checkbox"]').forEach((checkbox) => {
    checkbox.addEventListener("change", function () {
      // جمع جميع التقييمات المختارة
      const selectedRatings = Array.from(
        document.querySelectorAll('input[type="checkbox"]:checked')
      ).map((cb) => parseInt(cb.value)); // تحويل القيم إلى أرقام

      // استدعاء الدورات بناءً على التقييمات
      loadCoursesByRating(selectedRatings);
    });
  });
});
//////////////////////////////////////////////////////////////////////////////////////////////////
document
  .getElementById("newsletterForm")
  .addEventListener("submit", async function (event) {
    event.preventDefault(); // منع إعادة تحميل الصفحة

    // الحصول على البريد الإلكتروني من المدخل
    const email = document.getElementById("emailInput").value;

    // تحقق من صحة البريد الإلكتروني
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailPattern.test(email)) {
      Swal.fire({
        icon: "error",
        title: "خطأ",
        text: "يرجى إدخال بريد إلكتروني صالح.",
      });
      return;
    }

    // إعداد البيانات لاستخدام FormData
    const formData = new FormData();
    formData.append("email", email);

    try {
      // إرسال الطلب إلى الـ API
      const response = await fetch(
        "http://localhost:38146/api/Newsletter/DTONewsletter",
        {
          method: "POST",
          body: formData, // استخدام FormData
        }
      );

      if (response.ok) {
        // عرض رسالة نجاح
        Swal.fire({
          icon: "success",
          title: "تم الاشتراك بنجاح",
          text: "تم الاشتراك بنجاح في النشرة الإخبارية.",
        });
        document.getElementById("emailInput").value = ""; // إعادة تعيين الحقل
      } else {
        const errorData = await response.json();
        Swal.fire({
          icon: "error",
          title: "خطأ",
          text: errorData.message || "حدث خطأ أثناء الاشتراك. حاول مرة أخرى.",
        });
      }
    } catch (error) {
      // التعامل مع أخطاء الشبكة
      console.error("Error:", error);
      Swal.fire({
        icon: "error",
        title: "خطأ",
        text: "حدث خطأ في الشبكة. حاول مرة أخرى لاحقًا.",
      });
    }
  });
////////////////////////////////////////////////////////////////////////////////////////////////
// إضافة حدث النقر على زر البحث
document
  .querySelector(".btn.btn-warning")
  .addEventListener("click", function () {
    const searchQuery = document.querySelector(".form-control").value;

    // تحقق من إدخال المستخدم
    if (searchQuery.trim() === "") {
      alert("الرجاء إدخال عنوان الكورس للبحث.");
      return;
    }

    // إرسال الطلب إلى API البحث
    fetch(
      `http://localhost:38146/api/Courses/search?title=${encodeURIComponent(
        searchQuery
      )}`
    )
      .then((response) => response.json())
      .then((courses) => {
        // عرض النتائج في الـ Modal
        populateSearchResults(courses);
      })
      .catch((error) => console.error("Error fetching courses:", error));
  });

// دالة لتعبئة نتائج البحث في الـ Modal
function populateSearchResults(courses) {
  const resultsContainer = document.getElementById("searchResultsContainer");
  let resultsHtml = "";

  // التحقق من وجود نتائج
  if (courses.length === 0) {
    resultsHtml = "<p>لم يتم العثور على نتائج.</p>";
  } else {
    courses.forEach((course) => {
      const stars = generateStars(course.rating); // دالة لتوليد النجوم بناءً على التقييم
      resultsHtml += `
        <div class="col-md-4 mb-4">
          <div class="card h-100">
            <img src="/backend/image/${course.imageUrl}" class="card-img-top" alt="${course.title}" />
            <div class="card-body">
              <h5 class="card-title">${course.title}</h5>
              <p class="card-text">${course.description}</p>
              <p class="card-text">${course.price} دينار</p>
              <p class="card-text">التقييم: ${stars}</p>
            </div>
            <button class="btn btn-light border-0 favorite-btn" style="background-color: transparent">
              <i class="fa fa-heart" style="color: #ff6b6b; font-size: 1.5em"></i>
            </button>
            <div class="card-footer btn btn-warning" style="background-color: orange">
              <a href="javascript:void(0);" onclick="goToDetails(${course.courseId})" style="text-decoration: none">
                <strong class="text-orange" style="color: white">اذهب الى التفاصيل</strong>
              </a>
            </div>
          </div>
        </div>
      `;
    });
  }

  resultsContainer.innerHTML = resultsHtml;

  // عرض الـ Modal
  const modal = new bootstrap.Modal(
    document.getElementById("searchResultsModal")
  );
  modal.show();
}

// دالة لتوليد النجوم بناءً على التقييم
function generateStars(rating) {
  let stars = "";
  for (let i = 0; i < 5; i++) {
    if (i < rating) {
      stars += '<i class="fa fa-star" style="color: orange;"></i>';
    } else {
      stars += '<i class="fa fa-star" style="color: #ddd;"></i>';
    }
  }
  return stars;
}

// دالة لتوجيه المستخدم إلى صفحة التفاصيل
function goToDetails(courseId) {
  window.location.href = `/course/details/${courseId}`;
}
