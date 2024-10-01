// Function to load all learning equipment
function loadLearningEquipment() {
  fetch("http://localhost:38146/api/LearningEquipment")
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok " + response.statusText);
      }
      return response.json();
    })
    .then((data) => {
      const equipmentContainer = document.getElementById("LearningEquipment");
      equipmentContainer.innerHTML = ""; // Clear the container before adding new content

      // Create equipment cards
      data.forEach((equipment) => {
        const equipmentCard = `
              <div class="col-md-4 mb-4">
                <div class="card h-100">
                  <img src="/backend/image/${equipment.imageUrl1}" class="card-img-top" alt="${equipment.name}" />
                  <div class="card-body">
                    <h5 class="card-title">${equipment.name}</h5>
                    <p class="card-text">${equipment.description}</p>
                    <p class="card-text">${equipment.price} دينار</p>
                  </div>
                  <button class="btn btn-light border-0 favorite-btn" style="background-color: transparent">
                    <i class="fa fa-heart" style="color: #ff6b6b; font-size: 1.5em"></i>
                  </button>
                  <div class="card-footer btn btn-warning" style="background-color: orange">
                    <a href="javascript:void(0);" onclick="goToDetails2(${equipment.equipmentId})" style="text-decoration: none">
                      <strong class="text-orange" style="color: white">اذهب الى التفاصيل</strong>
                    </a>
                  </div>
                </div>
              </div>`;
        equipmentContainer.innerHTML += equipmentCard;
      });
    })
    .catch((error) => console.error("Error fetching equipment:", error));
}

// Function to load learning equipment by selected categories
function loadLearningEquipmentByCategories(categoryIds) {
  const url = `http://localhost:38146/api/LearningEquipment/byCategories?${categoryIds
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
      const equipmentContainer = document.getElementById("LearningEquipment");
      equipmentContainer.innerHTML = ""; // Clear the container before adding new content

      // Create equipment cards based on selected categories
      data.forEach((equipment) => {
        const equipmentCard = `
              <div class="col-md-4 mb-4">
                <div class="card h-100">
                  <img src="/backend/image/${equipment.imageUrl1}" class="card-img-top" alt="${equipment.name}" />
                  <div class="card-body">
                    <h5 class="card-title">${equipment.name}</h5>
                    <p class="card-text">${equipment.description}</p>
                    <p class="card-text">${equipment.price} دينار</p>
                  </div>
                  <button class="btn btn-light border-0 favorite-btn" style="background-color: transparent">
                    <i class="fa fa-heart" style="color: #ff6b6b; font-size: 1.5em"></i>
                  </button>
                  <div class="card-footer btn btn-warning" style="background-color: orange">
                    <a href="javascript:void(0);" onclick="goToDetails2(${equipment.equipmentId})" style="text-decoration: none">
                      <strong class="text-orange" style="color: white">اذهب الى التفاصيل</strong>
                    </a>
                  </div>
                </div>
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
function goToDetails2(equipmentId) {
  localStorage.setItem("equipmentId", equipmentId);
  window.location.href = `/frontend/user/equipmentshopdetails.html?id=${equipmentId}`;
}

// Function to handle category filtering
document.addEventListener("DOMContentLoaded", function () {
  // Fetch categories from API
  fetch("http://localhost:38146/api/Category/Categories")
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      return response.json();
    })
    .then((categories) => {
      const categoryList = document.getElementById("categoryListequipmentshop");

      // Add "All" option
      const allItem = document.createElement("li");
      allItem.classList.add("list-group-item");
      allItem.innerHTML = '<input type="checkbox" id="allItem" /> الكل';
      categoryList.appendChild(allItem);

      const allItemCheckbox = document.getElementById("allItem");
      allItemCheckbox.addEventListener("click", function () {
        if (allItemCheckbox.checked) {
          loadLearningEquipment(); // Load all equipment when "All" is selected
          categoryList
            .querySelectorAll('input[type="checkbox"]')
            .forEach((checkbox) => {
              if (checkbox !== allItemCheckbox) {
                checkbox.checked = false; // Uncheck other categories
              }
            });
        }
      });

      // Add categories from API
      categories.forEach((category) => {
        const listItem = document.createElement("li");
        listItem.classList.add("list-group-item");
        listItem.innerHTML = `<input type="checkbox" value="${category.categoryId}" /> ${category.name}`;
        categoryList.appendChild(listItem);

        const checkbox = listItem.querySelector('input[type="checkbox"]');
        checkbox.addEventListener("click", function () {
          const selectedCategories = getSelectedCategories();
          if (selectedCategories.length > 0) {
            loadLearningEquipmentByCategories(selectedCategories); // Load equipment by selected categories
          } else {
            loadLearningEquipment(); // Load all equipment if no categories selected
          }
          allItemCheckbox.checked = false; // Uncheck "All" if a category is selected
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
    '#categoryListequipmentshop input[type="checkbox"]:checked'
  );
  const categoryIds = Array.from(checkboxes)
    .map((checkbox) => checkbox.value)
    .filter((id) => id !== "allItem"); // Exclude "All" checkbox
  return categoryIds;
}
function loadLearningEquipmentByRating(ratings) {
  if (ratings.length === 0) {
    const equipmentContainer = document.getElementById("LearningEquipment");
    equipmentContainer.innerHTML =
      "<p>لم يتم العثور على معدات بناءً على التقييمات المختارة.</p>";
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
      const equipmentContainer = document.getElementById("LearningEquipment");
      equipmentContainer.innerHTML = ""; // تفريغ الحاوية قبل إضافة المحتوى الجديد

      if (data.length === 0) {
        equipmentContainer.innerHTML =
          "<p>لم يتم العثور على معدات بناءً على التقييمات المختارة.</p>";
      } else {
        data.forEach((equipment) => {
          // إنشاء سلسلة النجوم بناءً على التقييم
          const stars = "⭐".repeat(equipment.rating); // تكرار النجوم بناءً على قيمة التقييم

          const equipmentCard = `
              <div class="col-md-4 mb-4">
                <div class="card h-100">
                  <img src="/backend/image/${equipment.imageUrl1}" class="card-img-top" alt="${equipment.name}" />
                  <div class="card-body">
                    <h5 class="card-title">${equipment.name}</h5>
                    <p class="card-text">${equipment.description}</p>
                    <p class="card-text">${equipment.price} دينار</p>
                    <p class="card-text">التقييم: ${stars}</p> <!-- عرض النجوم بناءً على التقييم -->
                  </div>
                  <button class="btn btn-light border-0 favorite-btn" style="background-color: transparent">
                    <i class="fa fa-heart" style="color: #ff6b6b; font-size: 1.5em"></i>
                  </button>
                  <div class="card-footer btn btn-warning" style="background-color: orange">
                    <a href="javascript:void(0);" onclick="goToDetails2(${equipment.equipmentId})" style="text-decoration: none">
                      <strong class="text-orange" style="color: white">اذهب الى التفاصيل</strong>
                    </a>
                  </div>
                </div>
              </div>
            `;
          equipmentContainer.innerHTML += equipmentCard;
        });
      }
    })
    .catch((error) => console.error("Error fetching equipment:", error));
}

function loadCoursesByPrice(minPrice, maxPrice) {
  const url = `http://localhost:38146/api/LearningEquipment/prices?minPrice=${
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
      const coursesContainer = document.getElementById("LearningEquipment");
      coursesContainer.innerHTML = ""; // تفريغ الحاوية قبل إضافة المحتوى الجديد

      // إنشاء بطاقات الدورات بناءً على السعر
      data.forEach((equipment) => {
        // إنشاء سلسلة النجوم بناءً على التقييم
        const stars = "⭐".repeat(equipment.rating);

        const courseCard = `
               <div class="col-md-4 mb-4">
                  <div class="card h-100">
                    <img src="/backend/image/${equipment.imageUrl1}" class="card-img-top" alt="${equipment.name}" />
                    <div class="card-body">
                      <h5 class="card-title">${equipment.name}</h5>
                      <p class="card-text">${equipment.description}</p>
                      <p class="card-text">${equipment.price} دينار</p>
                      <p class="card-text">التقييم: ${stars}</p> <!-- عرض النجوم بناءً على التقييم -->
                    </div>
                    <button class="btn btn-light border-0 favorite-btn" style="background-color: transparent">
                      <i class="fa fa-heart" style="color: #ff6b6b; font-size: 1.5em"></i>
                    </button>
                    <div class="card-footer btn btn-warning" style="background-color: orange">
                      <a href="javascript:void(0);" onclick="goToDetails(${equipment.equipmentId})" style="text-decoration: none">
                        <strong class="text-orange" style="color: white">اذهب الى التفاصيل</strong>
                      </a>
                    </div>
                  </div>
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
    const coursesContainer = document.getElementById("LearningEquipment");
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
      const coursesContainer = document.getElementById("LearningEquipment");

      coursesContainer.innerHTML = ""; // تفريغ الحاوية قبل إضافة المحتوى الجديد

      // إنشاء بطاقات الدورات بناءً على التقييم
      if (data.length === 0) {
        coursesContainer.innerHTML =
          "<p>لم يتم العثور على دورات بناءً على التقييمات المختارة.</p>";
      } else {
        data.forEach((equipment) => {
          // إنشاء سلسلة النجوم بناءً على التقييم
          const stars = "⭐".repeat(equipment.rating); // تكرار النجوم بناءً على قيمة التقييم

          const courseCard = `
             <div class="col-md-4 mb-4">
                <div class="card h-100">
                  <img src="/backend/image/${equipment.imageUrl1}" class="card-img-top" alt="${equipment.name}" />
                  <div class="card-body">
                    <h5 class="card-title">${equipment.name}</h5>
                    <p class="card-text">${equipment.description}</p>
                    <p class="card-text">${equipment.price} دينار</p>
                    <p class="card-text">التقييم: ${stars}</p> <!-- عرض النجوم بناءً على التقييم -->
                  </div>
                  <button class="btn btn-light border-0 favorite-btn" style="background-color: transparent">
                    <i class="fa fa-heart" style="color: #ff6b6b; font-size: 1.5em"></i>
                  </button>
                  <div class="card-footer btn btn-warning" style="background-color: orange">
                    <a href="javascript:void(0);" onclick="goToDetails2(${equipment.equipmentId})" style="text-decoration: none">
                      <strong class="text-orange" style="color: white">اذهب الى التفاصيل</strong>
                    </a>
                  </div>
                </div>
              </div>
            `;
          coursesContainer.innerHTML += courseCard;
        });
      }
    })
    .catch((error) => console.error("Error fetching courses:", error));
}
