// جلب الأحداث من API
fetch("http://localhost:38146/api/Event")
  .then((response) => response.json())
  .then((events) => {
    const eventsContainer = document.getElementById("events-container");

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
