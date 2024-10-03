let totalPrice = 0;
let couponApplied = false;
let discountValue = 0;
//بدي اعمل انه فيه نوعين من الخصم اذا اول مره عم يشتري من عندي ف ف اله DISCOUNT بقيمه 20%
//ام المره الثانيه قيمه الخصم 10بالمئه

const validCoupons = {
  DISCOUNT10: 10,
  SAVE20: 20,
};

// استرجاع userID من API باستخدام email المخزن في localStorage
async function fetchUserId() {
  try {
    const email = localStorage.getItem("email");
    if (!email) return null; // إذا لم يكن هناك تسجيل دخول
    const response = await fetch(
      `http://localhost:38146/api/Users/email/${encodeURIComponent(email)}`
    );
    const data = await response.json();
    return data.userId; // تأكد من أن userId موجود في البيانات
  } catch (error) {
    console.error("Error fetching user ID:", error);
    return null;
  }
}

// استرجاع المنتجات اليدوية من API باستخدام userID
async function fetchHandmadeProducts(userID) {
  try {
    const response = await fetch(
      `http://localhost:38146/api/CardItemHandmadeProduct/card/${userID}`
    );
    const products = await response.json();
    return products;
  } catch (error) {
    console.error("Error fetching handmade products from API:", error);
    return [];
  }
}

// استرجاع المعدات التعليمية من API باستخدام userID
async function fetchLearningEquipment(userID) {
  try {
    const response = await fetch(
      `http://localhost:38146/api/CardItemLearningEquipment/card/${userID}`
    );
    const products = await response.json();
    return products;
  } catch (error) {
    console.error("Error fetching learning equipment from API:", error);
    return [];
  }
}

// استرجاع التفاصيل الإضافية للمنتجات باستخدام productId أو equipmentId
async function fetchProductDetails(productId, category) {
  const url =
    category === "Handmade"
      ? `http://localhost:38146/api/Handmade_Products/${productId}`
      : `http://localhost:38146/api/LearningEquipment/${productId}`;

  try {
    const response = await fetch(url);
    const productDetails = await response.json();
    return productDetails;
  } catch (error) {
    console.error("Error fetching product details:", error);
    return null;
  }
}

// Function to delete a product from localStorage
function removeFromLocalStorage(productId) {
  // Get the current cart from localStorage
  let cart = JSON.parse(localStorage.getItem("cart")) || [];

  // Filter out the product with the given productId
  cart = cart.filter((product) => product.productId !== productId);

  // Update the localStorage with the new cart array
  localStorage.setItem("cart", JSON.stringify(cart));
}

// Display products in the table
async function displayProducts(products) {
  const tbody = document.getElementById("product-table-body");
  tbody.innerHTML = ""; // Clear the table

  for (let index = 0; index < products.length; index++) {
    const product = products[index];
    const productId = product.equipmentId || product.productId;
    const category = product.category;

    // If details are missing, fetch them from API
    let productName = product.productName || product.name;
    let imageUrl = product.product?.image_url_1 || product.imageUrl;

    if (!productName || !imageUrl) {
      const productDetails = await fetchProductDetails(productId, category);
      if (productDetails) {
        productName = productDetails.name || productName;
        imageUrl = productDetails.image_url_1 || "default-image.jpg";
      } else {
        imageUrl = "default-image.jpg";
      }
    }

    const productPrice = product.productPrice || product.price || 0;

    const row = `
      <tr data-product-id="${productId}">
        <td>${index + 1}</td>
        <td><img src="${imageUrl}" alt="${productName}" style="width: 100px; height: auto;" /></td>
        <td>${productName}</td>
        <td class="unit-price">${productPrice} JD</td>
        <td>${product.quantity}</td>
        <td><button class="btn btn-danger remove-product"><i class="fa fa-times"></i></button></td>
      </tr>
    `;
    tbody.innerHTML += row;
  }

  setupEventListeners(); // Setup delete buttons
}

// Load products from both API and localStorage
async function loadProducts() {
  const userID = await fetchUserId(); // Get userID if the user is logged in

  let allProducts = [];

  if (userID) {
    const handmadeProducts = await fetchHandmadeProducts(userID);
    const learningEquipment = await fetchLearningEquipment(userID);

    // Merge handmade and learning equipment products into one array
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
  }

  // Add products from localStorage
  const localStorageProducts = JSON.parse(localStorage.getItem("cart")) || [];
  allProducts = [
    ...allProducts,
    ...localStorageProducts.map((product) => ({
      ...product,
      category: "Local",
    })),
  ];

  // Display all products
  await displayProducts(allProducts);
  updateTotalPrice(); // Update total price
}

// Set up event listeners for removing products
function setupEventListeners() {
  document.querySelectorAll(".remove-product").forEach(function (btn) {
    btn.addEventListener("click", async function () {
      const productId = this.closest("tr").getAttribute("data-product-id");
      const category = this.closest("tr").getAttribute("data-category");

      // Remove the product from the table
      this.closest("tr").remove();

      // If the product is from localStorage, remove it from localStorage
      if (category === "Local") {
        removeFromLocalStorage(productId);
      } else {
        // Remove the product from the API if it's not from localStorage
        if (category === "Handmade") {
          await deleteProductFromAPI(
            `http://localhost:38146/api/CardItemHandmadeProduct/${productId}`
          );
        } else if (category === "LearningEquipment") {
          await deleteProductFromAPI(
            `http://localhost:38146/api/CardItemLearningEquipment/${productId}`
          );
        }
      }

      // Update total price after removal
      updateTotalPrice();
    });
  });
}

// Delete product from API
async function deleteProductFromAPI(url) {
  try {
    const response = await fetch(url, { method: "DELETE" });
    if (!response.ok) {
      throw new Error("Failed to delete product from API");
    }
    console.log("Product deleted from API successfully");
  } catch (error) {
    console.error("Error deleting product from API:", error);
  }
}

// Update the total price displayed on the page
function updateTotalPrice() {
  let total = 0;
  document.querySelectorAll(".unit-price").forEach(function (priceElement) {
    total += parseFloat(priceElement.textContent.replace(" JD", ""));
  });

  if (couponApplied) {
    let discount = total * (discountValue / 100);
    total -= discount;
  }

  document.getElementById("total-price").textContent = `$${total.toFixed(2)}`;
}

// Load products when the page is loaded
window.onload = loadProducts;
