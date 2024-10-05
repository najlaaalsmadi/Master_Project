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
    return await response.json();
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
    return await response.json();
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
        imageUrl = "default-image.jpg";
      }
    }

    const productPrice = product.productPrice || product.price || 0;

    const row = `
      <tr data-product-id="${productId}" data-category="${category}">
        <td>${index + 1}</td>
        <td><img src="/backend/image/${imageUrl}" alt="${productName}" style="width: 100px; height: auto;" /></td>
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

// Apply coupon logic
async function applyCoupon(couponCode) {
  if (validCoupons[couponCode]) {
    const userId = await fetchUserId();
    const hasPurchased = await checkPurchaseHistory(userId);

    // Set discount value based on purchase history
    discountValue = hasPurchased
      ? validCoupons[couponCode]
      : validCoupons[couponCode];
    couponApplied = true;
    updateTotalPrice();
  } else {
    alert("Invalid coupon code.");
  }
}

// Check if the user has made any purchases
async function checkPurchaseHistory(userId) {
  // Implement API call to check if the user has made any purchases
  try {
    const response = await fetch(
      `http://localhost:38146/api/PurchaseHistory/${userId}`
    );
    if (response.ok) {
      const purchases = await response.json();
      return purchases.length > 0; // Return true if there are purchases
    }
    return false;
  } catch (error) {
    console.error("Error checking purchase history:", error);
    return false;
  }
}

// Load products when the page is loaded
window.onload = loadProducts;
