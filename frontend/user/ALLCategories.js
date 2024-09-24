document.addEventListener("DOMContentLoaded", function () {
  const apiUrl = "http://localhost:38146/api/Category/Categories";
  const categoriesDropdown = document.getElementById("categoriesDropdown");

  if (!categoriesDropdown) {
    console.error("Element with id 'categoriesDropdown' not found");
    return;
  }

  fetch(apiUrl)
    .then((response) => {
      console.log("API response status:", response.status);

      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      return response.json();
    })
    .then((categories) => {
      console.log("Fetched categories:", categories);
      categoriesDropdown.innerHTML = ""; // Clear existing items

      // Add 'All Courses' option
      const allCoursesItem = document.createElement("li");
      allCoursesItem.innerHTML = `<a class="dropdown-item" href="/frontend/user/Courses.html">الكل</a>`;
      categoriesDropdown.appendChild(allCoursesItem);

      // Add categories to dropdown
      categories.forEach((category) => {
        const listItem = document.createElement("li");
        localStorage.setItem("categoryId123", category.categoryId);
        listItem.innerHTML = `<a class="dropdown-item" href="/frontend/user/CoursesCategories.html?categoryId=${category.categoryId}">${category.name}</a>`;
        categoriesDropdown.appendChild(listItem);
      });
    })
    .catch((error) => {
      console.error("Error fetching categories:", error);
      categoriesDropdown.innerHTML =
        '<li><p class="dropdown-item">حدث خطأ أثناء تحميل الأقسام</p></li>';
    });
});

// document.addEventListener("DOMContentLoaded", function () {
//   // Fetch categories from API
//   fetch("http://localhost:38146/api/Category/Categories")
//     .then((response) => {
//       if (!response.ok) {
//         throw new Error("Network response was not ok");
//       }
//       return response.json();
//     })
//     .then((categories) => {
//       const categoryList = document.getElementById("categoryList");

//       // Add "All" option
//       const allItem = document.createElement("li");
//       allItem.classList.add("list-group-item");
//       allItem.innerHTML = '<input type="checkbox" id="allItem" /> الكل';
//       categoryList.appendChild(allItem);

//       const allItemCheckbox = document.getElementById("allItem");
//       allItemCheckbox.addEventListener("click", function () {
//         if (allItemCheckbox.checked) {
//           loadCourses(); // Load all courses when "All" is selected
//           categoryList
//             .querySelectorAll('input[type="checkbox"]')
//             .forEach((checkbox) => {
//               if (checkbox !== allItemCheckbox) {
//                 checkbox.checked = false; // Uncheck other categories
//               }
//             });
//         }
//       });

//       // Add categories from API
//       categories.forEach((category) => {
//         const listItem = document.createElement("li");
//         listItem.classList.add("list-group-item");
//         listItem.innerHTML = `<input type="checkbox" value="${category.categoryId}" /> ${category.name}`;
//         categoryList.appendChild(listItem);

//         const checkbox = listItem.querySelector('input[type="checkbox"]');
//         checkbox.addEventListener("click", function () {
//           localStorage.setItem(
//             `batoolItem${category.categoryId}`,
//             category.categoryId
//           );
//           const selectedCategories = getSelectedCategories();
//           if (selectedCategories.length > 0) {
//             loadCoursesByCategories(selectedCategories); // Load courses by selected categories
//           } else {
//             loadCourses(); // Load all courses if no categories selected
//           }
//           allItemCheckbox.checked = false; // Uncheck "All" if a category is selected
//         });
//       });
//     })
//     .catch((error) => {
//       console.error("Error fetching categories:", error);
//     });
// });

// Function to get selected categories
function getSelectedCategories() {
  const checkboxes = document.querySelectorAll(
    '#categoryList input[type="checkbox"]:not(#allItem)'
  );
  const selectedCategories = [];
  checkboxes.forEach((checkbox) => {
    if (checkbox.checked) {
      selectedCategories.push(checkbox.value);
    }
  });
  return selectedCategories;
}

// Function to load all courses
function loadCourses() {
  fetch("http://localhost:38146/api/Courses/all")
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok " + response.statusText);
      }
      return response.json();
    })
    .then((data) => {
      const coursesContainer = document.getElementById("courses-container");
      coursesContainer.innerHTML = ""; // Clear the container before adding new content

      // Create course cards
      data.forEach((course) => {
        const courseCard = `
          <div class="col-md-4 mb-4">
            <div class="card h-100">
              <img src="${course.imageUrl}" class="card-img-top" alt="${course.title}" />
              <div class="card-body">
                <h5 class="card-title">${course.title}</h5>
                <p class="card-text">${course.description}</p>
                <p class="card-text">${course.price}دينار</p>
              </div>
              <button class="btn btn-light border-0 favorite-btn" style="background-color: transparent">
                <i class="fa fa-heart" style="color: #ff6b6b; font-size: 1.5em"></i>
              </button>
              <div class="card-footer btn btn-warning" style="background-color: orange">
                <a href="javascript:void(0);" onclick="اذهب الى تفاصيل(${course.courseId})" style="text-decoration: none">
                  <strong class="text-orange" style="color: white"></strong>
                </a>اذهب الى تفاصيل
              </div>
            </div>
          </div>
        `;
        coursesContainer.innerHTML += courseCard;
      });
    })
    .catch((error) => console.error("Error fetching courses:", error));
}
// Function to load courses by selected category from localStorage
function loadCoursesByCategory() {
  debugger;
  // Get the category ID from localStorage
  const categoryId = localStorage.getItem("categoryId123");
  console.log("categoryId123", categoryId);
  if (!categoryId) {
    console.error("No category ID found in localStorage");
    return;
  }

  const url = `http://localhost:38146/api/Courses/categoryId/${categoryId}`;

  fetch(url)
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok " + response.statusText);
      }
      return response.json();
    })
    .then((data) => {
      console.log("data123", data);
      const coursesContainer = document.getElementById("courses-container");
      coursesContainer.innerHTML = ""; // Clear the container before adding new content

      // Create course cards based on selected category
      data.forEach((course) => {
        const courseCard = `
          <div class="col-md-4 mb-4">
            <div class="card h-100">
              <img src="${course.imageUrl}" class="card-img-top" alt="${course.title}" />
              <div class="card-body">
                <h5 class="card-title">${course.title}</h5>
                <p class="card-text">${course.description}</p>
                <p class="card-text">$${course.price}</p>
              </div>
              <button class="btn btn-light border-0 favorite-btn" style="background-color: transparent">
                <i class="fa fa-heart" style="color: #ff6b6b; font-size: 1.5em"></i>
              </button>
              <div class="card-footer btn btn-warning" style="background-color: orange">
                <a href="javascript:void(0);" onclick="goToDetails(${course.courseId})" style="text-decoration: none">
                  <strong class="text-orange" style="color: white">اذهب الى تفاصيل</strong>
                </a>
              </div>
            </div>
          </div>
        `;
        coursesContainer.innerHTML += courseCard;
      });
    })
    .catch((error) =>
      console.error("Error fetching courses by category:", error)
    );
}

// Call the function to load courses when the page loads
document.addEventListener("DOMContentLoaded", loadCoursesByCategory);

// Function to go to the course details page
function goToDetails(courseId) {
  localStorage.setItem("course_id", courseId);
  window.location.href = `/frontend/user/CourseDetails.html?id=${courseId}`;
}

// Load all courses when the page is opened
window.onload = loadCourses;

document.addEventListener("DOMContentLoaded", function () {
  // إضافة الأحداث لخيارات تصفية السعر
  document.querySelectorAll('input[name="price"]').forEach((radio) => {
    radio.addEventListener("change", function () {
      let minPrice = null;
      let maxPrice = null;

      if (this.value === "under100") {
        maxPrice = 99;
      } else if (this.value === "100to300") {
        minPrice = 100;
        maxPrice = 300;
      }

      loadCoursesByPrice(minPrice, maxPrice); // استدعاء الدورات بناءً على السعر
    });
  });
});

// تحميل الدورات حسب السعر
function loadCoursesByPrice(minPrice, maxPrice) {
  const url = `http://localhost:38146/api/Courses/prices?minPrice=${
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
      const coursesContainer = document.getElementById("courses-container");
      coursesContainer.innerHTML = ""; // تفريغ الحاوية قبل إضافة المحتوى الجديد

      // إنشاء بطاقات الدورات بناءً على السعر
      data.forEach((course) => {
        const courseCard = `
          <div class="col-md-4 mb-4">
            <div class="card h-100">
              <img src="${course.imageUrl}" class="card-img-top" alt="${course.title}" />
              <div class="card-body">
                <h5 class="card-title">${course.title}</h5>
                <p class="card-text">${course.description}</p>
                <p class="card-text">${course.price} دينار</p>
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
    const coursesContainer = document.getElementById("courses-container");
    coursesContainer.innerHTML =
      "<p>لم يتم العثور على دورات بناءً على التقييمات المختارة.</p>";
    return;
  }

  const url = `http://localhost:38146/api/Courses/ratings?ratings=${ratings}`;

  fetch(url)
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok " + response.statusText);
      }
      return response.json();
    })
    .then((data) => {
      const coursesContainer = document.getElementById("courses-container");
      coursesContainer.innerHTML = ""; // تفريغ الحاوية قبل إضافة المحتوى الجديد

      // إنشاء بطاقات الدورات بناءً على التقييم
      if (data.length === 0) {
        coursesContainer.innerHTML =
          "<p>لم يتم العثور على دورات بناءً على التقييمات المختارة.</p>";
      } else {
        data.forEach((course) => {
          // إنشاء سلسلة النجوم بناءً على التقييم
          const stars = "⭐".repeat(course.rating); // تكرار النجوم بناءً على قيمة التقييم

          const courseCard = `
            <div class="col-md-4 mb-4">
              <div class="card h-100">
                <img src="${course.imageUrl}" class="card-img-top" alt="${course.title}" />
                <div class="card-body">
                  <h5 class="card-title">${course.title}</h5>
                  <p class="card-text">${course.description}</p>
                  <p class="card-text">${course.price} دينار</p>
                  <p class="card-text">التقييم: ${stars}</p> <!-- عرض النجوم بناءً على التقييم -->
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
          coursesContainer.innerHTML += courseCard;
        });
      }
    })
    .catch((error) => console.error("Error fetching courses:", error));
}
