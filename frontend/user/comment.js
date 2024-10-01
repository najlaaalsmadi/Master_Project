// إرسال التعليق إلى الـ API
document
  .getElementById("commentForm")
  .addEventListener("submit", async function (event) {
    event.preventDefault();

    const reviewerName = document.getElementById("reviewerName").value;
    const commentText = document.getElementById("commentText").value;

    const commentData = {
      commentText: commentText,
      name: reviewerName,
    };

    try {
      const response = await fetch(
        "http://localhost:38146/api/CommentsCoursesBefore",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(commentData),
        }
      );

      if (response.ok) {
        Swal.fire({
          icon: "success",
          title: "تم إرسال تعليقك بنجاح!",
          showConfirmButton: false,
          timer: 1500,
        });

        // تحديث التعليقات
        fetchComments();
        document.getElementById("commentForm").reset();
      } else {
        const errorMessage = await response.text();
        Swal.fire({
          icon: "error",
          title: "حدث خطأ",
          text: `حدث خطأ: ${errorMessage}`,
        });
      }
    } catch (error) {
      Swal.fire({
        icon: "error",
        title: "خطأ في الاتصال",
        text: "حدث خطأ أثناء إرسال تعليقك.",
      });
    }
  });

// استرجاع التعليقات من الـ API
async function fetchComments() {
  try {
    const response = await fetch(
      "http://localhost:38146/api/CommentsCoursesBefore/GetCommentsRandom"
    );
    if (response.ok) {
      const comments = await response.json();
      displayComments(comments);
    } else {
      console.error("Failed to fetch comments");
    }
  } catch (error) {
    console.error("Error:", error);
  }
}

// عرض التعليقات والردود في الصفحة
async function displayComments(comments) {
  const commentsSection = document.getElementById("commentsSection");
  commentsSection.innerHTML = ""; // تفريغ المحتوى السابق

  for (const comment of comments) {
    const repliesHtml = await fetchRepliesForComment(comment.commentId);

    const commentHtml = `
      <div class="comment mb-3 p-3 bg-light rounded">
        <strong>${comment.name}:</strong>
        <p>${comment.commentText}</p>
        <button class="btn btn-link" onclick="toggleReplyForm(${comment.commentId})">رد</button>
        
        <div id="replyForm${comment.commentId}" class="reply-form mt-3" style="display: none;">
          <form onsubmit="event.preventDefault(); handleReplySubmit(${comment.commentId})">
            <div class="mb-2">
              <input type="text" class="form-control" id="replyName${comment.commentId}" placeholder="أدخل اسمك" required />
            </div>
            <div class="mb-2">
              <textarea class="form-control" id="replyText${comment.commentId}" rows="2" placeholder="أدخل ردك" required></textarea>
            </div>
            <button type="submit" class="btn btn-primary btn-sm">إرسال الرد</button>
          </form>
        </div>

        <div class="replies mt-2">
          ${repliesHtml}
        </div>
      </div>
    `;
    commentsSection.innerHTML += commentHtml;
  }
}

// استرجاع الردود لتظهر أسفل التعليق
async function fetchRepliesForComment(commentId) {
  try {
    const response = await fetch(
      `http://localhost:38146/api/ResponsesCommentsCoursesBefore/comment/${commentId}`
    );

    if (response.ok) {
      const replies = await response.json();
      return replies
        .map(
          (reply) => `
        <div class="reply mt-2 ms-4">
          <strong>${reply.name}:</strong>
          <p>${reply.responseText}</p>
        </div>
      `
        )
        .join("");
    } else {
      return "";
    }
  } catch (error) {
    console.error("Error fetching replies:", error);
    return "";
  }
}

// عرض أو إخفاء نموذج الرد
function toggleReplyForm(commentId) {
  const replyForm = document.getElementById(`replyForm${commentId}`);
  replyForm.style.display =
    replyForm.style.display === "none" ? "block" : "none";
}

// إرسال الرد إلى الـ API
async function handleReplySubmit(commentId) {
  const replyName = document.getElementById(`replyName${commentId}`).value;
  const replyText = document.getElementById(`replyText${commentId}`).value;

  const replyData = {
    commentId: commentId,
    responseText: replyText,
    name: replyName,
  };

  try {
    const response = await fetch(
      "http://localhost:38146/api/ResponsesCommentsCoursesBefore",
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(replyData),
      }
    );

    if (response.ok) {
      Swal.fire({
        icon: "success",
        title: "تم إرسال ردك بنجاح!",
        showConfirmButton: false,
        timer: 1500,
      });

      // إعادة تعيين نموذج الرد
      document.getElementById(`replyName${commentId}`).value = "";
      document.getElementById(`replyText${commentId}`).value = "";

      // تحديث التعليقات بعد إرسال الرد
      fetchComments();
    } else {
      const errorMessage = await response.text();
      Swal.fire({
        icon: "error",
        title: "حدث خطأ",
        text: `حدث خطأ: ${errorMessage}`,
      });
    }
  } catch (error) {
    Swal.fire({
      icon: "error",
      title: "خطأ في الاتصال",
      text: "حدث خطأ أثناء إرسال ردك.",
    });
  }
}

// تحميل التعليقات عند فتح الصفحة
window.onload = function () {
  fetchComments();
};
// عرض التعليقات في البوب أب مع الردود
async function showAllComments() {
  try {
    const response = await fetch(
      "http://localhost:38146/api/CommentsCoursesBefore"
    );
    if (response.ok) {
      const comments = await response.json();

      let commentsHtml = await Promise.all(
        comments.map(async (comment) => {
          const repliesHtml = await fetchRepliesForCommentInPopup(
            comment.commentId
          );
          return `
                <div class="comment mb-3 p-3 bg-light rounded">
                  <strong>${comment.name}:</strong>
                  <p>${comment.commentText}</p>
                  <button class="btn btn-link reply-btn" data-comment-id="${comment.commentId}">رد</button>
                  
                  <div id="replyForm${comment.commentId}" class="reply-form mt-3" style="display: none;">
                    <form onsubmit="event.preventDefault(); handleReplySubmit(${comment.commentId})">
                      <div class="mb-2">
                        <input type="text" class="form-control" id="replyName${comment.commentId}" placeholder="أدخل اسمك" required />
                      </div>
                      <div class="mb-2">
                        <textarea class="form-control" id="replyText${comment.commentId}" rows="2" placeholder="أدخل ردك" required></textarea>
                      </div>
                      <button type="submit" class="btn btn-primary btn-sm">إرسال الرد</button>
                    </form>
                  </div>
    
                  <div class="replies mt-2" id="repliesForComment${comment.commentId}">
                    ${repliesHtml}
                  </div>
                </div>
              `;
        })
      );

      Swal.fire({
        title: "كافة التعليقات",
        html: commentsHtml.join(""),
        showCloseButton: true,
        width: "80%", // عرض أكبر للبوبب
        height: "100%",
        customClass: {
          popup: "scrollable-popup", // لإضافة تمرير عند وجود تعليقات كثيرة
        },
        confirmButtonText: "إغلاق",
        didRender: () => {
          // إضافة الأحداث بعد تحميل البوب أب
          document.querySelectorAll(".reply-btn").forEach((button) => {
            button.addEventListener("click", function () {
              const commentId = this.getAttribute("data-comment-id");
              toggleReplyForm(commentId);
            });
          });
        },
      });
    } else {
      Swal.fire({
        icon: "error",
        title: "خطأ",
        text: "حدث خطأ أثناء استرجاع التعليقات.",
      });
    }
  } catch (error) {
    Swal.fire({
      icon: "error",
      title: "خطأ في الاتصال",
      text: "حدث خطأ أثناء استرجاع التعليقات.",
    });
  }
}

// استرجاع الردود للتعليق المحدد في البوب أب
async function fetchRepliesForCommentInPopup(commentId) {
  try {
    const response = await fetch(
      `http://localhost:38146/api/ResponsesCommentsCoursesBefore/comment/${commentId}`
    );

    if (response.ok) {
      const replies = await response.json();
      return replies
        .map(
          (reply) => `
            <div class="reply mt-2 ms-4">
              <strong>${reply.name}:</strong>
              <p>${reply.responseText}</p>
            </div>
          `
        )
        .join("");
    } else {
      return "";
    }
  } catch (error) {
    console.error("Error fetching replies:", error);
    return "";
  }
}

// التحكم في عرض أو إخفاء نموذج الرد
function toggleReplyForm(commentId) {
  const replyForm = document.getElementById(`replyForm${commentId}`);
  replyForm.style.display =
    replyForm.style.display === "none" ? "block" : "none";
}
