// Function to fetch categories from the API
async function fetchCategories() {
  try {
    const response = await fetch(
      "http://localhost:38146/api/Category/Categories"
    );
    const categories = await response.json();
    populateTable(categories);
  } catch (error) {
    console.error("Error fetching categories:", error);
  }
}

// Function to populate the table with category data
function populateTable(categories) {
  const tableBody = document.getElementById("categoryTableBody");
  tableBody.innerHTML = ""; // Clear existing content

  categories.forEach((category) => {
    const row = document.createElement("tr");

    row.innerHTML = `
      <td>${category.name}</td>
      <td>${category.description}</td>
      <td>
          <a class="btn btn-edit btn-sm" href="/frontend/admin/category/updatecategory.html?id=${category.categoryId}">تعديل</a>
          <br /><br /><br />
          <a class="btn btn-delete btn-sm" onclick="deleteCategory(${category.categoryId})">حذف</a>
      </td>
  `;

    tableBody.appendChild(row);
  });
}

// Fetch categories when the page loads
fetchCategories();
////////////////////////////////////////////////////////////////////////////////////////////
// Function to load the category data for editing
async function loadCategoryData(id) {
  try {
    const response = await fetch(
      `http://localhost:38146/api/Category/Categories/${id}`
    );
    if (!response.ok) throw new Error("فشل في تحميل بيانات الفئة");
    const category = await response.json();
    document.getElementById("categoryId").value = category.categoryId;
    document.getElementById("categoryName").value = category.name;
    document.getElementById("categoryDescription").value = category.description;
  } catch (error) {
    console.error("خطأ في تحميل بيانات الفئة:", error);
  }
}

// Function to handle form submission
document
  .getElementById("updateCategoryForm")
  .addEventListener("submit", async (event) => {
    event.preventDefault(); // Prevent default form submission
    const id = document.getElementById("categoryId").value;
    const updatedCategory = {
      categoryId: id,
      name: document.getElementById("categoryName").value,
      description: document.getElementById("categoryDescription").value,
    };

    try {
      const response = await fetch(
        `http://localhost:38146/api/Category/Categories/${id}`,
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(updatedCategory),
        }
      );

      if (response.ok) {
        // Use SweetAlert to show success message
        swal("نجاح!", "تم تعديل الفئة بنجاح", "success").then(() => {
          // Redirect to viewcategory.html after successful update
          window.location.href = "viewcategory.html"; // Update this path as necessary
        });
      } else {
        swal("خطأ!", "حدث خطأ أثناء تعديل الفئة", "error");
      }
    } catch (error) {
      console.error("خطأ في تعديل الفئة:", error);
    }
  });

// Get the category ID from the URL
const urlParams = new URLSearchParams(window.location.search);
const categoryId = urlParams.get("id");

// Load the category data when the page loads
if (categoryId) {
  loadCategoryData(categoryId);
}
//////////////////////////////////////////////////////////
// Function to delete a category
async function deleteCategory(categoryId) {
  // استخدام SweetAlert2 لتأكيد الحذف
  Swal.fire({
    title: "هل أنت متأكد؟",
    text: "سيتم حذف هذه الفئة بشكل دائم!",
    icon: "warning",
    showCancelButton: true,
    confirmButtonText: "نعم، احذفها!",
    cancelButtonText: "إلغاء",
    dangerMode: true,
  }).then(async (result) => {
    if (result.isConfirmed) {
      // عرض مؤشر الحذف
      Swal.fire({
        title: "جاري الحذف...",
        text: "يرجى الانتظار.",
        icon: "info",
        showConfirmButton: false,
        allowOutsideClick: false,
      });

      try {
        const response = await fetch(
          `http://localhost:38146/api/Category/Categories/${categoryId}`,
          {
            method: "DELETE",
          }
        );

        if (response.ok) {
          Swal.fire("تم الحذف!", "تم حذف الفئة بنجاح.", "success").then(() => {
            window.location.reload(); // إعادة تحميل الصفحة بعد الحذف
          });
        } else {
          Swal.fire("خطأ!", "حدث خطأ أثناء حذف الفئة.", "error");
        }
      } catch (error) {
        console.error("خطأ في حذف الفئة:", error);
        Swal.fire("خطأ!", "حدث خطأ أثناء حذف الفئة.", "error");
      }
    }
  });
}

////////////////////////////////////////////////
