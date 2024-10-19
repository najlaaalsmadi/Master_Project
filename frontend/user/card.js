// let totalPrice = 0;
// let couponApplied = false;
// let discountValue = 0;

// // Define valid coupons
// const validCoupons = {
//   DISCOUNT10: 10,
//   SAVE20: 20,
// };

// // Fetch user ID from the API using the email stored in localStorage
// async function fetchUserId() {
//   try {
//     const email = localStorage.getItem("email");
//     if (!email) return null; // If there's no login
//     const response = await fetch(
//       `http://localhost:38146/api/Users/email/${encodeURIComponent(email)}`
//     );
//     const data = await response.json();
//     return data.userId; // Ensure userId exists in the data
//   } catch (error) {
//     console.error("Error fetching user ID:", error);
//     return null;
//   }
// }

// // Fetch handmade products from the API using userID
// async function fetchHandmadeProducts(userID) {
//   try {
//     const response = await fetch(
//       `http://localhost:38146/api/CardItemHandmadeProduct/card/${userID}`
//     );

//     // Check if the response is ok
//     if (!response.ok) {
//       throw new Error(
//         `Failed to fetch handmade products: ${response.statusText}`
//       );
//     }

//     const data = await response.json();

//     // Ensure data is an array
//     if (!Array.isArray(data)) {
//       console.error("Expected an array but received:", data);
//       return []; // Return an empty array if data is not valid
//     }

//     return data;
//   } catch (error) {
//     console.error("Error fetching handmade products from API:", error);
//     return [];
//   }
// }

// // Fetch learning equipment from the API using userID
// async function fetchLearningEquipment(userID) {
//   try {
//     const response = await fetch(
//       `http://localhost:38146/api/CardItemLearningEquipment/card/${userID}`
//     );

//     // Check if the response is ok
//     if (!response.ok) {
//       throw new Error(
//         `Failed to fetch learning equipment: ${response.statusText}`
//       );
//     }

//     const data = await response.json();

//     // Ensure data is an array
//     if (!Array.isArray(data)) {
//       console.error("Expected an array but received:", data);
//       return []; // Return an empty array if data is not valid
//     }

//     return data;
//   } catch (error) {
//     console.error("Error fetching learning equipment from API:", error);
//     return [];
//   }
// }

// // Fetch additional product details using productId or equipmentId
// async function fetchProductDetails(productId, category) {
//   const url =
//     category === "Handmade"
//       ? `http://localhost:38146/api/Handmade_Products/${productId}`
//       : `http://localhost:38146/api/LearningEquipment/${productId}`;

//   return await fetchProduct(url);
// }

// // General function to fetch data from a given URL
// async function fetchProduct(url) {
//   try {
//     const response = await fetch(url);
//     if (!response.ok) throw new Error("Network response was not ok");
//     return await response.json();
//   } catch (error) {
//     console.error("Error fetching product details:", error);
//     return null;
//   }
// }

// // Remove a product from localStorage
// function removeFromLocalStorage(productId) {
//   let cart = JSON.parse(localStorage.getItem("cart")) || [];
//   cart = cart.filter((product) => product.productId !== productId);
//   localStorage.setItem("cart", JSON.stringify(cart));
// }

// // Delete product from the API
// async function deleteProductFromAPI(url) {
//   try {
//     const response = await fetch(url, {
//       method: "DELETE",
//     });

//     if (!response.ok) {
//       throw new Error("Failed to delete the product");
//     }
//   } catch (error) {
//     console.error("Error deleting product from API:", error);
//   }
// }

// // Display products in the table
// async function displayProducts(products) {
//   const tbody = document.getElementById("product-table-body");
//   tbody.innerHTML = ""; // Clear the table

//   for (let index = 0; index < products.length; index++) {
//     const product = products[index];
//     const productId = product.equipmentId || product.productId;
//     const category = product.category;

//     let productName = product.productName || product.name;
//     let imageUrl = product.product?.imageUrl1 || product.imageUrl1;

//     if (!productName || !imageUrl) {
//       const productDetails = await fetchProductDetails(productId, category);
//       if (productDetails) {
//         productName = productDetails.name || productName;
//         imageUrl = productDetails.imageUrl1;
//       } else {
//         imageUrl = "default-image.jpg"; // Default image if not found
//       }
//     }

//     const productPrice = product.productPrice || product.price || 0;

//     const row = `
//       <tr data-product-id="${productId}" data-category="${category}">
//         <td>${index + 1}</td>
//         <td><img src="/backend/image/${imageUrl}" alt="${productName}" style="width: 100px; height: auto;" /></td>
//         <td>${productName}</td>
//         <td class="unit-price">${productPrice} JD</td>
//         <td>
//           <button class="btn btn-secondary decrease-quantity">-</button>
//           <input type="text" value="${
//             product.quantity
//           }" class="product-quantity" style="width: 50px; text-align: center;" />
//           <button class="btn btn-secondary increase-quantity">+</button>
//         </td>
//         <td><button class="btn btn-danger remove-product"><i class="fa fa-times"></i></button></td>
//       </tr>
//     `;
//     tbody.innerHTML += row;
//   }

//   setupEventListeners(); // Setup delete buttons and quantity change events
// }

// // Load products from both API and localStorage
// async function loadProducts() {
//   const userID = await fetchUserId(); // Get userID if the user is logged in

//   let allProducts = [];

//   if (userID) {
//     const [handmadeProducts, learningEquipment] = await Promise.all([
//       fetchHandmadeProducts(userID),
//       fetchLearningEquipment(userID),
//     ]);

//     // Ensure that both products arrays are valid
//     if (Array.isArray(handmadeProducts) && Array.isArray(learningEquipment)) {
//       allProducts = [
//         ...handmadeProducts.map((product) => ({
//           ...product,
//           category: "Handmade",
//         })),
//         ...learningEquipment.map((product) => ({
//           ...product,
//           category: "LearningEquipment",
//         })),
//       ];
//     } else {
//       console.error("Invalid product data received");
//     }
//   }

//   // Add products from localStorage
//   const localStorageProducts = JSON.parse(localStorage.getItem("cart")) || [];
//   allProducts = [
//     ...allProducts,
//     ...localStorageProducts.map((product) => ({
//       ...product,
//       category: "Local",
//     })),
//   ];

//   // Display all products
//   await displayProducts(allProducts);
//   updateTotalPrice(); // Update total price
// }

// // Set up event listeners for removing products and quantity changes
// function setupEventListeners() {
//   // Event listener for removing products
//   document.querySelectorAll(".remove-product").forEach(function (btn) {
//     btn.addEventListener("click", async function () {
//       const row = this.closest("tr");
//       const productId = row.getAttribute("data-product-id");
//       const category = row.getAttribute("data-category");

//       // Remove the product from the table
//       row.remove();

//       // Remove the product from localStorage or API based on category
//       if (category === "Local") {
//         removeFromLocalStorage(productId);
//       } else {
//         const apiUrl =
//           category === "Handmade"
//             ? `http://localhost:38146/api/CardItemHandmadeProduct/${productId}`
//             : `http://localhost:38146/api/CardItemLearningEquipment/${productId}`;

//         await deleteProductFromAPI(apiUrl); // Call delete API
//       }

//       updateTotalPrice(); // Update total price after removal
//     });
//   });

//   // Event listeners for increasing and decreasing quantities
//   document.querySelectorAll(".increase-quantity").forEach(function (btn) {
//     btn.addEventListener("click", function () {
//       const input = this.closest("tr").querySelector(".product-quantity");
//       input.value = parseInt(input.value) + 1;
//       updateTotalPrice(); // Update total price after quantity change
//     });
//   });

//   document.querySelectorAll(".decrease-quantity").forEach(function (btn) {
//     btn.addEventListener("click", function () {
//       const input = this.closest("tr").querySelector(".product-quantity");
//       if (parseInt(input.value) > 1) {
//         input.value = parseInt(input.value) - 1;
//       }
//       updateTotalPrice(); // Update total price after quantity change
//     });
//   });

//   // Event listener for manual quantity input
//   document.querySelectorAll(".product-quantity").forEach(function (input) {
//     input.addEventListener("input", function () {
//       if (parseInt(this.value) < 1 || isNaN(parseInt(this.value))) {
//         this.value = 1; // Set minimum quantity to 1
//       }
//       updateTotalPrice(); // Update total price after manual input
//     });
//   });
// }

// // Update the total price displayed on the page
// function updateTotalPrice() {
//   let total = 0;

//   // جمع الأسعار لكل منتج
//   document.querySelectorAll("tr").forEach(function (row) {
//     const quantity = row.querySelector(".product-quantity")
//       ? parseInt(row.querySelector(".product-quantity").value)
//       : 0;
//     const unitPrice = row.querySelector(".unit-price")
//       ? parseFloat(
//           row.querySelector(".unit-price").textContent.replace(" JD", "")
//         )
//       : 0;

//     total += quantity * unitPrice;
//   });

//   // Apply discount if a coupon has been applied
//   if (couponApplied) {
//     total = total * (1 - discountValue / 100);
//   }

//   // Display the total price
//   document.getElementById("total-price").textContent = `Total: ${total.toFixed(
//     2
//   )} JD`;
// }

// // Apply coupon function
// function applyCoupon() {
//   const couponInput = document.getElementById("coupon-code").value.trim();
//   if (validCoupons[couponInput]) {
//     discountValue = validCoupons[couponInput];
//     couponApplied = true;
//     updateTotalPrice();
//     document.getElementById(
//       "coupon-message"
//     ).textContent = `Coupon applied! You got a ${discountValue}% discount.`;
//   } else {
//     document.getElementById("coupon-message").textContent =
//       "Invalid coupon code.";
//   }
// }

// // Load products when the page is ready
// window.addEventListener("DOMContentLoaded", loadProducts);

// // Apply coupon on button click
// document
//   .getElementById("apply-coupon-btn")
//   .addEventListener("click", applyCoupon);
let totalPrice = 0;
let couponApplied = false;
let discountValue = 0;

// Define valid coupons
const validCoupons = {
  DISCOUNT10: 10,
  SAVE20: 20,
};

// Fetch user ID from the API using the email stored in localStorage
async function fetchUserId() {
  try {
    const email = localStorage.getItem("email");
    if (!email) return null; // If there's no login
    const response = await fetch(
      `http://localhost:38146/api/Users/email/${encodeURIComponent(email)}`
    );
    const data = await response.json();
    return data.userId; // Ensure userId exists in the data
  } catch (error) {
    console.error("Error fetching user ID:", error);
    return null;
  }
}

// Fetch handmade products from the API using userID
async function fetchHandmadeProducts(userID) {
  try {
    const response = await fetch(
      `http://localhost:38146/api/CardItemHandmadeProduct/card/${userID}`
    );

    if (!response.ok) {
      throw new Error(
        `Failed to fetch handmade products: ${response.statusText}`
      );
    }

    const data = await response.json();

    if (!Array.isArray(data)) {
      console.error("Expected an array but received:", data);
      return []; // Return an empty array if data is not valid
    }

    return data;
  } catch (error) {
    console.error("Error fetching handmade products from API:", error);
    return [];
  }
}

// Fetch learning equipment from the API using userID
async function fetchLearningEquipment(userID) {
  try {
    const response = await fetch(
      `http://localhost:38146/api/CardItemLearningEquipment/card/${userID}`
    );

    if (!response.ok) {
      throw new Error(
        `Failed to fetch learning equipment: ${response.statusText}`
      );
    }

    const data = await response.json();

    if (!Array.isArray(data)) {
      console.error("Expected an array but received:", data);
      return []; // Return an empty array if data is not valid
    }

    return data;
  } catch (error) {
    console.error("Error fetching learning equipment from API:", error);
    return [];
  }
}

// Fetch additional product details using productId or equipmentId
async function fetchProductDetails(productId, category) {
  const url =
    category === "Handmade"
      ? `http://localhost:38146/api/Handmade_Products/${productId}`
      : `http://localhost:38146/api/LearningEquipment/${productId}`;

  return await fetchProduct(url);
}

// General function to fetch data from a given URL
async function fetchProduct(url) {
  try {
    const response = await fetch(url);
    if (!response.ok) throw new Error("Network response was not ok");
    return await response.json();
  } catch (error) {
    console.error("Error fetching product details:", error);
    return null;
  }
}

// Remove a product from localStorage
function removeFromLocalStorage(productId) {
  let cart = JSON.parse(localStorage.getItem("cart")) || [];
  cart = cart.filter((product) => product.productId !== productId);
  localStorage.setItem("cart", JSON.stringify(cart));
}

// Update quantity in the API for a specific product
async function updateProductQuantity(productId, category, newQuantity) {
  const url =
    category === "Handmade"
      ? `http://localhost:38146/api/CardItemHandmadeProduct/${productId}`
      : `http://localhost:38146/api/CardItemLearningEquipment/${productId}`;

  try {
    const response = await fetch(url, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ quantity: newQuantity }), // Adjust this based on your API
    });

    if (!response.ok) {
      throw new Error("Failed to update product quantity");
    }
  } catch (error) {
    console.error("Error updating product quantity in API:", error);
  }
}

// Delete product from the API
async function deleteProductFromAPI(url) {
  try {
    const response = await fetch(url, {
      method: "DELETE",
    });

    if (!response.ok) {
      throw new Error("Failed to delete the product");
    }
  } catch (error) {
    console.error("Error deleting product from API:", error);
  }
}

// Display products in the table
async function displayProducts(products) {
  const tbody = document.getElementById("product-table-body");
  tbody.innerHTML = ""; // Clear the table

  for (let index = 0; index < products.length; index++) {
    const product = products[index];
    const productId = product.equipmentId || product.productId;
    const category = product.category;

    let productName = product.productName || product.name;
    let imageUrl = product.product?.imageUrl1 || product.imageUrl1;

    if (!productName || !imageUrl) {
      const productDetails = await fetchProductDetails(productId, category);
      if (productDetails) {
        productName = productDetails.name || productName;
        imageUrl = productDetails.imageUrl1;
      } else {
        imageUrl = "backend/image/default-image.jpg"; // Default image if not found
      }
    }

    const productPrice = product.productPrice || product.price || 0;

    const row = `
      <tr data-product-id="${productId}" data-category="${category}">
        <td>${index + 1}</td>
        <td><img src="/backend/image/${imageUrl}" alt="${productName}" style="width: 100px; height: auto;" /></td>
        <td>${productName}</td>
        <td class="unit-price">${productPrice} JD</td>
        <td>
          <button class="btn btn-secondary decrease-quantity">-</button>
          <input type="text" value="${
            product.quantity
          }" class="product-quantity" style="width: 50px; text-align: center;" />
          <button class="btn btn-secondary increase-quantity">+</button>
        </td>
        <td><button class="btn btn-danger remove-product"><i class="fa fa-times"></i></button></td>
      </tr>
    `;
    tbody.innerHTML += row;
  }

  setupEventListeners(); // Setup delete buttons and quantity change events
}

// Load products from both API and localStorage
async function loadProducts() {
  const userID = await fetchUserId(); // Get userID if the user is logged in

  let allProducts = [];

  if (userID) {
    const [handmadeProducts, learningEquipment] = await Promise.all([
      fetchHandmadeProducts(userID),
      fetchLearningEquipment(userID),
    ]);

    if (Array.isArray(handmadeProducts) && Array.isArray(learningEquipment)) {
      allProducts = [
        ...handmadeProducts.map((product) => ({
          ...product,
          category: "Handmade",
        })),
        ...learningEquipment.map((product) => ({
          ...product,
          category: "LearningEquipment",
        })),
      ];
    } else {
      console.error("Invalid product data received");
    }
  }

  const localStorageProducts = JSON.parse(localStorage.getItem("cart")) || [];
  allProducts = [
    ...allProducts,
    ...localStorageProducts.map((product) => ({
      ...product,
      category: "Local",
    })),
  ];

  await displayProducts(allProducts);
  updateTotalPrice(); // Update total price
}

// Set up event listeners for removing products and quantity changes
function setupEventListeners() {
  // Event listener for removing products
  document.querySelectorAll(".remove-product").forEach(function (btn) {
    btn.addEventListener("click", async function () {
      const row = this.closest("tr");
      const productId = row.getAttribute("data-product-id");
      const category = row.getAttribute("data-category");

      row.remove(); // Remove the product from the table

      if (category === "Local") {
        removeFromLocalStorage(productId); // Remove from localStorage
      } else {
        const apiUrl =
          category === "Handmade"
            ? `http://localhost:38146/api/CardItemHandmadeProduct/${productId}`
            : `http://localhost:38146/api/CardItemLearningEquipment/${productId}`;
        await deleteProductFromAPI(apiUrl); // Call delete API
      }

      updateTotalPrice(); // Update total price after removal
    });
  });

  // Event listeners for increasing and decreasing quantities
  document.querySelectorAll(".increase-quantity").forEach(function (btn) {
    btn.addEventListener("click", function () {
      const row = this.closest("tr");
      const input = row.querySelector(".product-quantity");
      const newQuantity = parseInt(input.value) + 1;
      input.value = newQuantity;
      updateProductQuantity(
        row.getAttribute("data-product-id"),
        row.getAttribute("data-category"),
        newQuantity
      ); // Update quantity in API
      updateTotalPrice(); // Update total price after quantity change
    });
  });

  document.querySelectorAll(".decrease-quantity").forEach(function (btn) {
    btn.addEventListener("click", function () {
      const row = this.closest("tr");
      const input = row.querySelector(".product-quantity");
      const currentQuantity = parseInt(input.value);
      if (currentQuantity > 1) {
        const newQuantity = currentQuantity - 1;
        input.value = newQuantity;
        updateProductQuantity(
          row.getAttribute("data-product-id"),
          row.getAttribute("data-category"),
          newQuantity
        ); // Update quantity in API
        updateTotalPrice(); // Update total price after quantity change
      }
    });
  });
}

// Update total price based on current products in the table
function updateTotalPrice() {
  const unitPrices = document.querySelectorAll(".unit-price");
  const quantities = document.querySelectorAll(".product-quantity");
  totalPrice = 0;

  for (let i = 0; i < unitPrices.length; i++) {
    const price = parseFloat(unitPrices[i].innerText);
    const quantity = parseInt(quantities[i].value);
    totalPrice += price * quantity;
  }

  document.getElementById("total-price").innerText = `${totalPrice.toFixed(
    2
  )} JD`;
  localStorage.setItem("pricecard", totalPrice);
  // Update the total price display
}

document.addEventListener("DOMContentLoaded", loadProducts);
document
  .getElementById("paypalButton")
  .addEventListener("click", async function () {
    const isLoggedIn = localStorage.getItem("email"); // افتراض وجود البريد الإلكتروني في الـ LocalStorage كدليل لتسجيل الدخول

    if (!isLoggedIn) {
      // توجيه المستخدم إلى صفحة تسجيل الدخول إذا لم يكن مسجلًا
      window.location.href = "/frontend/user/login.html"; // استبدل هذا بالرابط الصحيح لصفحة تسجيل الدخول
    } else {
      // التحقق من تفاصيل تسجيل الدخول والانتقال إلى صفحة الدفع
      window.location.href = "/frontend/user/paymentcard.html"; // صفحة الدفع تشبه PayPal
    }
  });
