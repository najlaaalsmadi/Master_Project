debugger;
const API_BASE_URL = "http://localhost:38146/api";

// استخراج ID المنتج من معلمات الـ URL
const urlParams = new URLSearchParams(window.location.search);
const id = urlParams.get("id"); // تأكد أن الـ URL يحتوي على ?id=2 مثلاً
debugger;
// استدعاء بيانات المنتج من API
fetch(`${API_BASE_URL}/LearningEquipment/${id}`)
  .then((response) => {
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    return response.json();
  })
  .then((data) => {
    const defaultImage = `/backend/image/default.png`;

    // تعيين الصورة الرئيسية والصور المصغرة
    document.getElementById("mainImage").src = data.imageUrl1
      ? `/backend/image/${data.imageUrl1}`
      : defaultImage;
    document.getElementById("thumbnail1").src = data.imageUrl1
      ? `/backend/image/${data.imageUrl1}`
      : defaultImage;
    document.getElementById("thumbnail2").src = data.imageUrl2
      ? `/backend/image/${data.imageUrl2}`
      : defaultImage;
    document.getElementById("thumbnail3").src = data.imageUrl3
      ? `/backend/image/${data.imageUrl3}`
      : defaultImage;

    // إضافة وظائف لتغيير الصورة الرئيسية عند النقر على الصور المصغرة
    document
      .getElementById("thumbnail1")
      .addEventListener("click", function () {
        document.getElementById("mainImage").src = data.imageUrl1
          ? `/backend/image/${data.imageUrl1}`
          : defaultImage;
      });
    document
      .getElementById("thumbnail2")
      .addEventListener("click", function () {
        document.getElementById("mainImage").src = data.imageUrl2
          ? `/backend/image/${data.imageUrl2}`
          : defaultImage;
      });
    document
      .getElementById("thumbnail3")
      .addEventListener("click", function () {
        document.getElementById("mainImage").src = data.imageUrl3
          ? `/backend/image/${data.imageUrl3}`
          : defaultImage;
      });

    // تعيين معلومات المنتج
    document.getElementById("productName").textContent = data.name;
    document.getElementById("productPrice").textContent = `${data.price} دينار`;
    document.getElementById("productRating").textContent =
      "★★★★★" + ` (${data.rating})`;
    document.getElementById("productDescription").textContent =
      data.description;
  })
  .catch((error) => {
    Swal.fire({
      title: "خطأ",
      text: "حدث خطأ أثناء استدعاء البيانات، يرجى المحاولة لاحقاً.",
      icon: "error",
    });
    console.error("Error fetching product data:", error);
  });

// استدعاء بيانات المنتجات ذات الصلة من API
fetch(`${API_BASE_URL}/LearningEquipment/random`)
  .then((response) => {
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    return response.json();
  })
  .then((data) => {
    const relatedProductsContainer = document.getElementById("relatedProducts");

    // إنشاء العناصر لكل منتج
    data.forEach((product) => {
      const productHTML = `
        <div class="col-md-4 mb-4">
          <a href="javascript:void(0);" onclick="goToDetails3(${product.equipmentId})" class="card text-decoration-none text-dark">
            <img src="/backend/image/${product.imageUrl1}" class="card-img-top img-fluid" alt="${product.name}" />
            <div class="card-body text-center">
              <h5 class="card-title">${product.name}</h5>
              <p class="card-text">${product.price} دينار</p>
              <p class="card-text">⭐⭐⭐⭐⭐ (${product.rating})</p>
            </div>
          </a>
        </div>`;
      relatedProductsContainer.innerHTML += productHTML;
    });
  })
  .catch((error) => {
    Swal.fire({
      title: "خطأ",
      text: "حدث خطأ أثناء استدعاء المنتجات ذات الصلة، يرجى المحاولة لاحقاً.",
      icon: "error",
    });
    console.error("Error fetching related products:", error);
  });

function goToDetails3(equipmentId) {
  localStorage.setItem("equipmentId1", equipmentId);
  window.location.href = `/frontend/user/equipmentshopdetails.html?id=${equipmentId}`;
}

// إضافة المنتج إلى السلة
document
  .getElementById("addToCartButton")
  .addEventListener("click", function (e) {
    e.preventDefault();
    debugger;
    const userEmail = localStorage.getItem("email");
    const productId = id;
    const productName = document.getElementById("productName").textContent;
    const productPrice = parseFloat(
      document
        .getElementById("productPrice")
        .textContent.replace(/[^0-9.]/g, "")
    );
    const productImage = document.getElementById("mainImage").src;
    const currentDate = new Date().toISOString();

    if (userEmail) {
      fetchUserIdAndAddToCart(
        userEmail,
        productId,
        productName,
        productPrice,
        productImage,
        currentDate
      );
    } else {
      addToLocalStorage(productId, productName, productPrice, productImage);

      Swal.fire({
        title: "تمت إضافة المنتج إلى السلة!",
        text: "يمكنك الذهاب إلى السلة لإتمام عملية الشراء أو مواصلة التسوق.",
        icon: "success",
        showCancelButton: true,
        confirmButtonText: "الذهاب إلى السلة",
        cancelButtonText: "مواصلة التسوق",
        reverseButtons: true,
      }).then((result) => {
        if (result.isConfirmed) {
          window.location.href = "/frontend/user/card.html";
        }
      });
    }
  });

function addToLocalStorage(productId, productName, productPrice, productImage) {
  let cart = JSON.parse(localStorage.getItem("cart")) || [];
  const userId = localStorage.getItem("userId") || 0; // Default to 0 if not logged in
  cart.push({
    cardId: userId, // Use userId or 0 as cardId
    productId,
    productName,
    quantity: 1, // افتراض كمية واحدة عند إضافة المنتج للسلة
    productPrice,
    productImage,
    addedAt: new Date().toISOString(),
  });
  localStorage.setItem("cart", JSON.stringify(cart));
}

// عند تسجيل الدخول
function handleLogin() {
  const userEmail = localStorage.getItem("email");

  if (!userEmail) {
    console.error("No email found in localStorage");
    return;
  }

  fetch(`${API_BASE_URL}/Users/email/${encodeURIComponent(userEmail)}`)
    .then((response) => {
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      return response.json();
    })
    .then((userData) => {
      debugger;
      const userId = userData.id || userData.userId;
      const cart = JSON.parse(localStorage.getItem("cart")) || [];
      console.log("cart", cart);
      if (cart.length > 0) {
        cart.forEach((product) => {
          addToCartAPI(
            product.productId,
            product.productName,
            product.productPrice,
            product.productImage,
            product.quantity,
            product.addedAt,
            userId
          );
        });
        localStorage.removeItem("cart");
      }
    })
    .catch((error) => {
      console.error("Error fetching user data:", error);
    });
}

function fetchUserIdAndAddToCart(
  userEmail,
  productId,
  productName,
  productPrice,
  productImage,
  addedAt
) {
  fetch(`${API_BASE_URL}/Users/email/${encodeURIComponent(userEmail)}`)
    .then((response) => {
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      return response.json();
    })
    .then((userData) => {
      const userId = userData.id || userData.userId;
      addToCartAPI(
        productId,
        productName,
        productPrice,
        productImage,
        1,
        addedAt,
        userId
      );
    })
    .catch((error) => {
      console.error("Error fetching user data:", error);
    });
}

function addToCartAPI(
  productId,
  productName,
  productPrice,
  productImage,
  quantity,
  addedAt,
  userId
) {
  fetch(`${API_BASE_URL}/CardItem`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      cardId: userId, // Set cardId equal to userId
      productId,
      quantity,
      price: productPrice,
      addedAt: addedAt,
      userId,
    }),
  })
    .then((cartItem) => {
      Swal.fire({
        title: "تمت إضافة المنتج إلى السلة!",
        text: "يمكنك الذهاب إلى السلة لإتمام عملية الشراء أو مواصلة التسوق.",
        icon: "success",
        showCancelButton: true,
        confirmButtonText: "الذهاب إلى السلة",
        cancelButtonText: "مواصلة التسوق",
        reverseButtons: true,
      }).then((result) => {
        if (result.isConfirmed) {
          window.location.href = "/frontend/user/card.html";
        }
      });
    })
    .catch((error) => {
      Swal.fire({
        title: "خطأ",
        text: "حدث خطأ أثناء استدعاء المنتجات ذات الصلة، يرجى المحاولة لاحقاً.",
        icon: "error",
      });
      console.error("Error fetching related products:", error);
    });
}
