// Function to load all learning equipment
function loadLearningEquipment() {
  fetch("http://localhost:38146/api/Handmade_Products")
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok " + response.statusText);
      }
      return response.json();
    })
    .then((data) => {
      const equipmentContainer = document.getElementById("HandmadeProducts");
      equipmentContainer.innerHTML = ""; // Clear the container before adding new content

      // Create equipment cards
      data.forEach((equipment) => {
        const stars = "⭐".repeat(equipment.rating); // تكرار النجوم بناءً على قيمة التقييم

        const equipmentCard = `

        <div class="col-md-4 mb-4">
<a href="javascript:void(0);" onclick="goToDetails21(${equipment.productId})" class="card text-decoration-none text-dark">

    <img src="/backend/image/${equipment.imageUrl1}" class="card-img-top" alt="${equipment.name} ">
    <div class="card-body text-center">
        <h5 class="card-title">${equipment.name}  </h5>
                    <p class="card-text">${equipment.price} دينار</p>
        <p class="card-text">${stars}</p>
        
    </div>
</a>
</div>`;
        equipmentContainer.innerHTML += equipmentCard;
      });
    })
    .catch((error) => console.error("Error fetching equipment:", error));
}

// Function to load learning equipment by selected categories
function loadLearningEquipmentByCategories(categoryIds) {
  const url = `http://localhost:38146/api/Handmade_Products/byCategories?${categoryIds
    .map((id) => `categoryIds=${id}`)
    .join("&")}`;
  fetch(url)
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok " + response.statusText);
      }
      return response.json();
    })
    .then((data) => {
      const equipmentContainer = document.getElementById("HandmadeProducts");
      equipmentContainer.innerHTML = ""; // مسح المحتوى الحالي قبل إضافة الجديد

      // إنشاء بطاقات للمعدات بناءً على الفئات المختارة
      data.forEach((equipment) => {
        const equipmentCard = `
          <div class="col-md-4 mb-4">
            <a href="javascript:void(0);" onclick="goToDetails21(${equipment.productId})" class="card text-decoration-none text-dark">
                <img src="/backend/image/${equipment.imageUrl1}" class="card-img-top" alt="${equipment.name}">
                <div class="card-body text-center">
                    <h5 class="card-title">${equipment.name}</h5>
                    <p class="card-text">${equipment.price} دينار</p>
                    <p class="card-text">${stars}</p>
                </div>
            </a>
          </div>`;
        equipmentContainer.innerHTML += equipmentCard;
      });
    })
    .catch((error) =>
      console.error("Error fetching equipment by category:", error)
    );
}

// Load all learning equipment when the page is opened
window.onload = loadLearningEquipment;
debugger;
function goToDetails21(productId) {
  localStorage.setItem("productId", productId);
  window.location.href = `/frontend/user/HandmadeShopdetils.html?id=${productId}`;
}

// Function to handle category filtering
document.addEventListener("DOMContentLoaded", function () {
  fetch("http://localhost:38146/api/Category/Categories")
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      return response.json();
    })
    .then((categories) => {
      const categoryList = document.getElementById("categoryHandmadeProducts");

      // إضافة خيار "الكل"
      const allItem = document.createElement("li");
      allItem.classList.add("list-group-item");
      allItem.innerHTML = '<input type="checkbox" id="allItem" /> الكل';
      categoryList.appendChild(allItem);

      const allItemCheckbox = document.getElementById("allItem");
      allItemCheckbox.addEventListener("click", function () {
        if (allItemCheckbox.checked) {
          loadLearningEquipment(); // تحميل جميع المعدات عند اختيار "الكل"
          categoryList
            .querySelectorAll('input[type="checkbox"]')
            .forEach((checkbox) => {
              if (checkbox !== allItemCheckbox) {
                checkbox.checked = false; // إلغاء اختيار الفئات الأخرى
              }
            });
        }
      });

      // إضافة الفئات من الـ API
      categories.forEach((category) => {
        const listItem = document.createElement("li");
        listItem.classList.add("list-group-item");
        listItem.innerHTML = `<input type="checkbox" value="${category.categoryId}" /> ${category.name}`;
        categoryList.appendChild(listItem);

        const checkbox = listItem.querySelector('input[type="checkbox"]');
        checkbox.addEventListener("click", function () {
          const selectedCategories = getSelectedCategories();
          if (selectedCategories.length > 0) {
            loadLearningEquipmentByCategories(selectedCategories); // تحميل المعدات بناءً على الفئات المختارة
          } else {
            loadLearningEquipment(); // تحميل جميع المعدات إذا لم يتم اختيار أي فئة
          }
          allItemCheckbox.checked = false; // إلغاء تحديد "الكل" إذا تم اختيار فئة
        });
      });
    })
    .catch((error) => {
      console.error("Error fetching categories:", error);
    });
});

// Function to get selected categories
function getSelectedCategories() {
  const checkboxes = document.querySelectorAll(
    '#categoryHandmadeProducts input[type="checkbox"]:checked'
  );
  const categoryIds = Array.from(checkboxes)
    .map((checkbox) => checkbox.value)
    .filter((id) => id !== "allItem"); // استثناء خيار "الكل"
  return categoryIds;
}

function loadLearningEquipmentByRating(ratings) {
  if (ratings.length === 0) {
    const equipmentContainer = document.getElementById("HandmadeProducts");
    equipmentContainer.innerHTML =
      "<p>لم يتم العثور على معدات بناءً على التقييمات المختارة.</p>";
    return;
  }

  const url = `http://localhost:38146/api/Handmade_Products/ratings?ratings=${ratings}`;

  fetch(url)
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok " + response.statusText);
      }
      return response.json();
    })
    .then((data) => {
      const equipmentContainer = document.getElementById("HandmadeProducts");
      equipmentContainer.innerHTML = ""; // تفريغ الحاوية قبل إضافة المحتوى الجديد

      if (data.length === 0) {
        equipmentContainer.innerHTML =
          "<p>لم يتم العثور على منتجات بناءً على التقييمات المختارة.</p>";
      } else {
        data.forEach((equipment) => {
          // إنشاء سلسلة النجوم بناءً على التقييم
          const stars = "⭐".repeat(equipment.rating); // تكرار النجوم بناءً على قيمة التقييم

          const equipmentCard = `
                
        <div class="col-md-4 mb-4">
<a href="javascript:void(0);" onclick="goToDetails21(${equipment.productId})" class="card text-decoration-none text-dark">

    <img src="/backend/image/${equipment.imageUrl1}" class="card-img-top" alt="${equipment.name} ">
    <div class="card-body text-center">
        <h5 class="card-title">${equipment.name}  </h5>
                    <p class="card-text">${equipment.price} دينار</p>
        <p class="card-text">${stars}</p>
        
    </div>
</a>
</div>
            `;
          equipmentContainer.innerHTML += equipmentCard;
        });
      }
    })
    .catch((error) => console.error("Error fetching equipment:", error));
}

function loadCoursesByPrice(minPrice, maxPrice) {
  const url = `http://localhost:38146/api/Handmade_Products/prices?minPrice=${
    minPrice || ""
  }&maxPrice=${maxPrice || ""}`;

  fetch(url)
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok " + response.statusText);
      }
      return response.json();
    })
    .then((data) => {
      const coursesContainer = document.getElementById("HandmadeProducts");
      coursesContainer.innerHTML = ""; // تفريغ الحاوية قبل إضافة المحتوى الجديد

      // إنشاء بطاقات الدورات بناءً على السعر
      data.forEach((equipment) => {
        // إنشاء سلسلة النجوم بناءً على التقييم
        const stars = "⭐".repeat(equipment.rating);

        const courseCard = `
               
        <div class="col-md-4 mb-4">
<a href="javascript:void(0);" onclick="goToDetails21(${equipment.productId})" class="card text-decoration-none text-dark">

    <img src="/backend/image/${equipment.imageUrl1}" class="card-img-top" alt="${equipment.name} ">
    <div class="card-body text-center">
        <h5 class="card-title">${equipment.name}  </h5>
                    <p class="card-text">${equipment.price} دينار</p>
        <p class="card-text">${stars}</p>
        
    </div>
</a>
</div>
            `;
        coursesContainer.innerHTML += courseCard;
      });
    })
    .catch((error) => console.error("Error fetching courses:", error));
}

document.addEventListener("DOMContentLoaded", function () {
  // إضافة الأحداث لخيارات تصفية التقييمات
  document.querySelectorAll(".rating-checkbox").forEach((checkbox) => {
    checkbox.addEventListener("change", function () {
      const selectedRatings = [];

      // جمع التقييمات المختارة
      document
        .querySelectorAll(".rating-checkbox:checked")
        .forEach((checkedBox) => {
          selectedRatings.push(checkedBox.value);
        });

      // تحميل الدورات بناءً على التقييمات المختارة
      loadCoursesByRating(selectedRatings);
    });
  });
});
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

// تحميل الدورات حسب التقييم
function loadCoursesByRating(ratings) {
  if (ratings.length === 0) {
    const coursesContainer = document.getElementById("HandmadeProducts");
    coursesContainer.innerHTML =
      "<p>لم يتم العثور على دورات بناءً على التقييمات المختارة.</p>";
    return;
  }

  const url = `http://localhost:38146/api/LearningEquipment/ratings?ratings=${ratings}`;

  fetch(url)
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok " + response.statusText);
      }
      return response.json();
    })
    .then((data) => {
      const coursesContainer = document.getElementById("HandmadeProducts");

      coursesContainer.innerHTML = ""; // تفريغ الحاوية قبل إضافة المحتوى الجديد

      // إنشاء بطاقات الدورات بناءً على التقييم
      if (data.length === 0) {
        coursesContainer.innerHTML =
          "<p>لم يتم العثور على المنتجات بناءً على التقييمات المختارة.</p>";
      } else {
        data.forEach((equipment) => {
          // إنشاء سلسلة النجوم بناءً على التقييم
          const stars = "⭐".repeat(equipment.rating); // تكرار النجوم بناءً على قيمة التقييم

          const courseCard = `
                
        <div class="col-md-4 mb-4">
<a href="javascript:void(0);" onclick="goToDetails21(${equipment.productId})" class="card text-decoration-none text-dark">

    <img src="/backend/image/${equipment.imageUrl1}" class="card-img-top" alt="${equipment.name} ">
    <div class="card-body text-center">
        <h5 class="card-title">${equipment.name}  </h5>
                    <p class="card-text">${equipment.price} دينار</p>
        <p class="card-text">${stars}</p>
        
    </div>
</a>
</div>
            `;
          coursesContainer.innerHTML += courseCard;
        });
      }
    })
    .catch((error) => console.error("Error fetching courses:", error));
}
