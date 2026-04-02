const state = {
    page: 1,
    size: 9,
    totalCount: 0,
    filters: { category: "", minPrice: "", maxPrice: "" },
    products: [],
    token: localStorage.getItem("affiliate_token") || "",
    user: readStoredJson("affiliate_user"),
    cart: null,
    orders: [],
    productModal: {
        productId: "",
        product: null,
        reviews: null
    }
};

const elements = {
    authMessage: document.getElementById("authMessage"),
    cartItems: document.getElementById("cartItems"),
    cartSummary: document.getElementById("cartSummary"),
    catalogMessage: document.getElementById("catalogMessage"),
    checkoutForm: document.getElementById("checkoutForm"),
    checkoutMessage: document.getElementById("checkoutMessage"),
    filtersForm: document.getElementById("filtersForm"),
    loadOrdersButton: document.getElementById("loadOrdersButton"),
    loadProductsButton: document.getElementById("loadProductsButton"),
    loginForm: document.getElementById("loginForm"),
    logoutButton: document.getElementById("logoutButton"),
    nextPageButton: document.getElementById("nextPageButton"),
    openAuthButton: document.getElementById("openAuthButton"),
    ordersList: document.getElementById("ordersList"),
    pageSizeSelect: document.getElementById("pageSizeSelect"),
    paginationLabel: document.getElementById("paginationLabel"),
    previousPageButton: document.getElementById("previousPageButton"),
    productGrid: document.getElementById("productGrid"),
    productCardTemplate: document.getElementById("productCardTemplate"),
    productModal: document.getElementById("productModal"),
    closeModalButton: document.getElementById("closeModalButton"),
    modalCategory: document.getElementById("modalCategory"),
    modalTitle: document.getElementById("modalTitle"),
    modalDescription: document.getElementById("modalDescription"),
    modalPrice: document.getElementById("modalPrice"),
    modalStock: document.getElementById("modalStock"),
    modalStars: document.getElementById("modalStars"),
    modalAverageNumber: document.getElementById("modalAverageNumber"),
    modalRatingLabel: document.getElementById("modalRatingLabel"),
    ratingPicker: document.getElementById("ratingPicker"),
    reviewForm: document.getElementById("reviewForm"),
    reviewMessage: document.getElementById("reviewMessage"),
    reviewsList: document.getElementById("reviewsList"),
    reviewItemTemplate: document.getElementById("reviewItemTemplate"),
    refreshReviewsButton: document.getElementById("refreshReviewsButton"),
    refreshProductsButton: document.getElementById("refreshProductsButton"),
    sessionDetail: document.getElementById("sessionDetail"),
    sessionState: document.getElementById("sessionState"),
    shopNowButton: document.getElementById("shopNowButton"),
    metricProducts: document.getElementById("metricProducts")
};

initialize();

function initialize() {
    bindEvents();
    handlePaymentReturn();
    syncAuthUi();
    fetchProducts();

    if (state.token) {
        loadCart();
        loadOrders();
    }
}

function bindEvents() {
    elements.filtersForm.addEventListener("submit", handleFilterSubmit);
    elements.loginForm.addEventListener("submit", handleLoginSubmit);
    elements.checkoutForm.addEventListener("submit", handleCheckoutSubmit);
    elements.previousPageButton.addEventListener("click", () => changePage(-1));
    elements.nextPageButton.addEventListener("click", () => changePage(1));
    elements.refreshProductsButton.addEventListener("click", () => fetchProducts());
    elements.loadProductsButton.addEventListener("click", () => fetchProducts());
    elements.loadOrdersButton.addEventListener("click", () => loadOrders());
    elements.logoutButton.addEventListener("click", logout);
    elements.shopNowButton.addEventListener("click", () => {
        document.querySelector(".catalog-section").scrollIntoView({ behavior: "smooth", block: "start" });
    });
    elements.openAuthButton.addEventListener("click", () => {
        document.querySelector(".auth-card").scrollIntoView({ behavior: "smooth", block: "start" });
        document.querySelector("input[name='email']").focus();
    });
    elements.pageSizeSelect.addEventListener("change", event => {
        state.size = Number(event.target.value);
        state.page = 1;
        fetchProducts();
    });

    elements.closeModalButton.addEventListener("click", closeProductModal);
    elements.productModal.addEventListener("click", event => {
        const target = event.target;
        if (target?.dataset?.modalClose === "true") closeProductModal();
    });
    elements.refreshReviewsButton.addEventListener("click", () => {
        if (state.productModal.productId) loadProductReviews(state.productModal.productId);
    });
    elements.reviewForm.addEventListener("submit", handleReviewSubmit);
    document.addEventListener("keydown", event => {
        if (event.key === "Escape" && !elements.productModal.classList.contains("hidden")) closeProductModal();
    });

    setupRatingPicker(5);
}

async function fetchProducts() {
    setCatalogMessage("Dang tai danh sach san pham...");

    const params = new URLSearchParams({
        page: String(state.page),
        size: String(state.size)
    });

    if (state.filters.category) params.set("category", state.filters.category);
    if (state.filters.minPrice) params.set("minPrice", state.filters.minPrice);
    if (state.filters.maxPrice) params.set("maxPrice", state.filters.maxPrice);

    try {
        const response = await fetch(`/api/v1/products?${params.toString()}`);
        if (!response.ok) throw new Error("Khong the tai du lieu san pham.");

        const payload = await response.json();
        state.products = payload.items || [];
        state.totalCount = payload.totalCount || 0;
        renderProducts();
        updatePagination(payload.page || state.page, payload.size || state.size, payload.totalCount || 0);
    } catch (error) {
        elements.productGrid.innerHTML = "";
        setCatalogMessage(error.message);
    }
}

function renderProducts() {
    elements.productGrid.innerHTML = "";
    elements.metricProducts.textContent = String(state.products.length);

    if (!state.products.length) {
        setCatalogMessage("Khong tim thay san pham phu hop voi bo loc hien tai.");
        return;
    }

    setCatalogMessage(`${state.totalCount} san pham trong catalog. Dang hien thi ${state.products.length} muc.`);

    state.products.forEach((product, index) => {
        const node = elements.productCardTemplate.content.cloneNode(true);
        const card = node.querySelector(".product-card");
        card.classList.add("fade-in");
        card.style.animationDelay = `${index * 40}ms`;

        node.querySelector(".category-chip").textContent = product.category || "General";
        const statusChip = node.querySelector(".status-chip");
        const isActive = product.status === 1;
        statusChip.textContent = isActive ? "Active" : "Inactive";
        statusChip.classList.toggle("inactive", !isActive);
        node.querySelector(".product-name").textContent = product.name;
        node.querySelector(".product-description").textContent = product.description || "Chua co mo ta.";
        node.querySelector(".product-price").textContent = formatCurrency(product.price);
        node.querySelector(".product-stock").textContent = `Ton kho: ${product.stock}`;

        const quantityInput = node.querySelector(".quantity-input");
        quantityInput.max = String(Math.max(product.stock, 1));

        const detailButton = node.querySelector(".view-detail-button");
        detailButton.addEventListener("click", () => openProductModal(product.id));

        const addButton = node.querySelector(".add-cart-button");
        addButton.disabled = !state.token || !isActive || product.stock <= 0;
        addButton.textContent = !isActive ? "Tam an" : product.stock > 0 ? "Them vao gio" : "Het hang";
        if (!state.token && isActive && product.stock > 0) {
            addButton.title = "Dang nhap role User de them vao gio hang.";
        }
        addButton.addEventListener("click", () => addToCart(product.id, quantityInput.value));

        elements.productGrid.appendChild(node);
    });
}

async function handleLoginSubmit(event) {
    event.preventDefault();
    const form = new FormData(event.currentTarget);
    setAuthMessage("Dang xac thuc...");

    try {
        const response = await fetch("/api/v1/auth/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                request: {
                    email: String(form.get("email") || ""),
                    password: String(form.get("password") || "")
                }
            })
        });

        if (!response.ok) {
            throw new Error("Dang nhap that bai. Kiem tra email, mat khau va role nguoi dung.");
        }

        const payload = await response.json();
        state.token = payload.accessToken || "";
        state.user = payload.infoUser || null;
        persistSession();
        syncAuthUi();
        await Promise.all([loadCart(), loadOrders()]);
        setAuthMessage("Dang nhap thanh cong. Gio hang va don hang da duoc dong bo.");
        event.currentTarget.reset();
    } catch (error) {
        setAuthMessage(error.message);
    }
}

async function addToCart(productId, quantityValue) {
    if (!state.token) {
        setAuthMessage("Can dang nhap role User truoc khi them vao gio hang.");
        document.querySelector(".auth-card").scrollIntoView({ behavior: "smooth", block: "center" });
        return;
    }

    const quantity = Number(quantityValue);
    if (!Number.isFinite(quantity) || quantity <= 0) {
        setCheckoutMessage("So luong khong hop le.");
        return;
    }

    setCheckoutMessage("Dang them san pham vao gio...");

    try {
        const response = await fetch("/api/v1/cart/add", {
            method: "POST",
            headers: authorizedJsonHeaders(),
            body: JSON.stringify({ productId, quantity })
        });

        if (response.status === 401 || response.status === 403) {
            throw new Error("Tai khoan hien tai khong co quyen them vao gio. Hay dang nhap bang role User.");
        }

        if (!response.ok) throw new Error(await readError(response, "Khong the them vao gio hang."));

        await loadCart();
        setCheckoutMessage("Da cap nhat gio hang.");
    } catch (error) {
        setCheckoutMessage(error.message);
    }
}

async function loadCart() {
    if (!state.token) {
        state.cart = null;
        renderCart();
        return;
    }

    try {
        const response = await fetch("/api/v1/cart", { headers: authorizedHeaders() });

        if (response.status === 404) {
            state.cart = { items: [] };
            renderCart();
            return;
        }

        if (!response.ok) throw new Error(await readError(response, "Khong the tai gio hang."));

        state.cart = await response.json();
        renderCart();
    } catch (error) {
        setCheckoutMessage(error.message);
    }
}

function renderCart() {
    const items = state.cart?.items || [];
    elements.cartSummary.textContent = `${items.length} mon`;

    if (!state.token) {
        elements.cartItems.innerHTML = "Dang nhap de tai gio hang.";
        return;
    }

    if (!items.length) {
        elements.cartItems.innerHTML = "Gio hang trong. Hay them mot vai san pham dang active.";
        return;
    }

    elements.cartItems.innerHTML = "";
    items.forEach(item => {
        const wrapper = document.createElement("article");
        wrapper.className = "cart-item";
        wrapper.innerHTML = `
            <div class="cart-item-header">
                <strong>${escapeHtml(item.name)}</strong>
                <span>${formatCurrency(item.price * item.quantity)}</span>
            </div>
            <div class="muted-text">${formatCurrency(item.price)} x ${item.quantity}</div>
            <div class="cart-item-controls">
                <input class="mini-input" type="number" min="1" value="${item.quantity}">
                <button class="ghost-button compact-button" type="button">Cap nhat</button>
            </div>
        `;

        const input = wrapper.querySelector("input");
        wrapper.querySelector("button").addEventListener("click", async () => {
            await updateCartItem(item.id, Number(input.value));
        });

        elements.cartItems.appendChild(wrapper);
    });
}

async function updateCartItem(itemId, quantity) {
    if (!Number.isFinite(quantity) || quantity <= 0) {
        setCheckoutMessage("So luong cap nhat phai lon hon 0.");
        return;
    }

    setCheckoutMessage("Dang cap nhat gio hang...");

    try {
        const response = await fetch(`/api/v1/cart/items/${itemId}`, {
            method: "PUT",
            headers: authorizedJsonHeaders(),
            body: JSON.stringify({ quantity })
        });

        if (!response.ok) throw new Error(await readError(response, "Khong the cap nhat gio hang."));

        await loadCart();
        setCheckoutMessage("Gio hang da duoc cap nhat.");
    } catch (error) {
        setCheckoutMessage(error.message);
    }
}

async function handleCheckoutSubmit(event) {
    event.preventDefault();
    if (!state.token) {
        setCheckoutMessage("Can dang nhap de checkout.");
        return;
    }

    const form = new FormData(event.currentTarget);
    const paymentMethod = String(form.get("paymentMethod") || "COD");
    setCheckoutMessage("Dang tao don hang...");

    try {
        if (paymentMethod === "VNPAY") {
            const response = await fetch("/api/v1/payments/vnpay/create", {
                method: "POST",
                headers: authorizedJsonHeaders(),
                body: JSON.stringify({
                    paymentMethod,
                    couponCode: String(form.get("couponCode") || "").trim() || null
                })
            });

            if (!response.ok) throw new Error(await readError(response, "Khong the khoi tao thanh toan VNPay."));

            const payment = await response.json();
            setCheckoutMessage("Dang chuyen sang cong thanh toan VNPay...");
            window.location.assign(payment.paymentUrl);
            return;
        }

        const response = await fetch("/api/v1/orders/checkout", {
            method: "POST",
            headers: authorizedJsonHeaders(),
            body: JSON.stringify({
                paymentMethod,
                couponCode: String(form.get("couponCode") || "").trim() || null
            })
        });

        if (!response.ok) throw new Error(await readError(response, "Checkout that bai."));

        const order = await response.json();
        setCheckoutMessage(`Da tao don ${order.id}. Thanh tien ${formatCurrency(order.finalAmount)}.`);
        await Promise.all([loadCart(), loadOrders()]);
        event.currentTarget.reset();
    } catch (error) {
        setCheckoutMessage(error.message);
    }
}

function handlePaymentReturn() {
    const params = new URLSearchParams(window.location.search);
    const paymentStatus = params.get("paymentStatus");
    if (!paymentStatus) return;

    const orderRef = params.get("orderRef");
    const responseCode = params.get("responseCode");
    if (paymentStatus === "success") {
        setCheckoutMessage(`VNPay thanh cong cho don ${orderRef || ""}.`);
    } else {
        setCheckoutMessage(`VNPay that bai hoac bi huy. Ma phan hoi: ${responseCode || "N/A"}.`);
    }
}

async function loadOrders() {
    if (!state.token) {
        state.orders = [];
        renderOrders();
        return;
    }

    try {
        const response = await fetch("/api/v1/orders", { headers: authorizedHeaders() });
        if (!response.ok) throw new Error(await readError(response, "Khong the tai lich su don hang."));

        state.orders = await response.json();
        renderOrders();
    } catch (error) {
        elements.ordersList.textContent = error.message;
    }
}

function renderOrders() {
    if (!state.token) {
        elements.ordersList.textContent = "Dang nhap de xem lich su don hang.";
        return;
    }

    if (!state.orders.length) {
        elements.ordersList.textContent = "Chua co don hang nao. Thu checkout de tao don dau tien.";
        return;
    }

    elements.ordersList.innerHTML = "";
    state.orders.slice(0, 6).forEach(order => {
        const wrapper = document.createElement("article");
        wrapper.className = "order-item";
        const orderLines = (order.items || []).map(item => `${escapeHtml(item.productName)} x${item.quantity}`).join(", ");

        wrapper.innerHTML = `
            <div class="order-item-header">
                <strong>#${String(order.id).slice(0, 8)}</strong>
                <span>${formatCurrency(order.finalAmount)}</span>
            </div>
            <div class="order-summary muted-text">${escapeHtml(orderLines || "Khong co chi tiet")}</div>
            <div class="muted-text">Coupon: ${escapeHtml(order.couponCode || "Khong dung")} | Giam: ${formatCurrency(order.discount || 0)}</div>
        `;
        elements.ordersList.appendChild(wrapper);
    });
}

function handleFilterSubmit(event) {
    event.preventDefault();
    state.page = 1;
    state.filters = {
        category: elements.filtersForm.category.value.trim(),
        minPrice: elements.filtersForm.minPrice.value.trim(),
        maxPrice: elements.filtersForm.maxPrice.value.trim()
    };
    fetchProducts();
}

function changePage(delta) {
    const lastPage = Math.max(1, Math.ceil(state.totalCount / state.size));
    const nextPage = state.page + delta;
    if (nextPage < 1 || nextPage > lastPage) return;
    state.page = nextPage;
    fetchProducts();
}

function updatePagination(page, size, totalCount) {
    state.page = page;
    state.size = size;
    const lastPage = Math.max(1, Math.ceil(totalCount / size));
    elements.paginationLabel.textContent = `Trang ${page} / ${lastPage}`;
    elements.previousPageButton.disabled = page <= 1;
    elements.nextPageButton.disabled = page >= lastPage;
}

async function openProductModal(productId) {
    state.productModal.productId = String(productId || "");
    state.productModal.product = null;
    state.productModal.reviews = null;

    elements.productModal.classList.remove("hidden");
    elements.productModal.setAttribute("aria-hidden", "false");

    elements.modalCategory.textContent = "San pham";
    elements.modalTitle.textContent = "Dang tai...";
    elements.modalDescription.textContent = "";
    elements.modalPrice.textContent = "";
    elements.modalStock.textContent = "";
    elements.modalAverageNumber.textContent = "0.0";
    renderStars(elements.modalStars, 0);
    elements.modalRatingLabel.textContent = "0 danh gia";
    elements.reviewsList.textContent = "Dang tai danh gia...";
    setReviewMessage("");

    try {
        await Promise.all([loadProductDetail(productId), loadProductReviews(productId)]);
    } catch (error) {
        setReviewMessage(error.message);
    }

    syncReviewFormState();
}

function closeProductModal() {
    elements.productModal.classList.add("hidden");
    elements.productModal.setAttribute("aria-hidden", "true");
    state.productModal.productId = "";
    state.productModal.product = null;
    state.productModal.reviews = null;
    setReviewMessage("");
}

async function loadProductDetail(productId) {
    const response = await fetch(`/api/v1/products/${productId}`);
    if (!response.ok) throw new Error("Khong the tai chi tiet san pham.");

    const product = await response.json();
    state.productModal.product = product;
    renderProductModal();
}

async function loadProductReviews(productId) {
    const response = await fetch(`/api/v1/products/${productId}/reviews?take=20`);
    if (!response.ok) throw new Error(await readError(response, "Khong the tai danh gia san pham."));

    state.productModal.reviews = await response.json();
    renderProductReviews();
}

function renderProductModal() {
    const product = state.productModal.product;
    if (!product) return;

    elements.modalTitle.textContent = product.name || "San pham";
    elements.modalCategory.textContent = product.category || "General";
    elements.modalDescription.textContent = product.description || "Chua co mo ta.";
    elements.modalPrice.textContent = formatCurrency(product.price);
    elements.modalStock.textContent = `Ton kho: ${product.stock ?? 0}`;

    const summary = state.productModal.reviews;
    if (summary) {
        const avg = Number(summary.averageRating || 0);
        elements.modalAverageNumber.textContent = avg.toFixed(1);
        renderStars(elements.modalStars, avg);
        elements.modalRatingLabel.textContent = `${summary.totalReviews || 0} danh gia`;
    }
}

function renderProductReviews() {
    const summary = state.productModal.reviews;
    renderProductModal();

    if (!summary || !(summary.items || []).length) {
        elements.reviewsList.textContent = "Chua co danh gia nao.";
        return;
    }

    elements.reviewsList.innerHTML = "";

    (summary.items || []).forEach(item => {
        const node = elements.reviewItemTemplate.content.cloneNode(true);
        const identity = node.querySelector(".review-user");
        const stars = node.querySelector(".stars");
        const comment = node.querySelector(".review-comment");
        const date = node.querySelector(".review-date");

        identity.textContent = formatReviewUser(item.userId);
        renderStars(stars, Number(item.rating || 0), true);
        comment.textContent = item.comment || "";
        date.textContent = formatDateTime(item.createdAt);

        elements.reviewsList.appendChild(node);
    });
}

function formatReviewUser(userId) {
    const currentUserId = getCurrentUserId();
    const shortId = String(userId || "").slice(0, 8);

    if (currentUserId && String(currentUserId).toLowerCase() === String(userId || "").toLowerCase()) {
        return `Ban (${shortId})`;
    }

    return `User ${shortId}`;
}

function getCurrentUserId() {
    return state.user?.id || state.user?.Id || null;
}

function renderStars(container, rating, small = false) {
    const value = Math.max(0, Math.min(5, Number(rating || 0)));
    const filled = Math.round(value);

    container.classList.toggle("small", Boolean(small));
    container.innerHTML = "";

    for (let index = 1; index <= 5; index += 1) {
        const star = document.createElement("span");
        star.className = `star${index <= filled ? " filled" : ""}`;
        star.setAttribute("aria-hidden", "true");
        container.appendChild(star);
    }
}

function setupRatingPicker(initialRating) {
    elements.ratingPicker.innerHTML = "";

    for (let rating = 1; rating <= 5; rating += 1) {
        const button = document.createElement("button");
        button.type = "button";
        button.className = "star-choice";
        button.setAttribute("role", "radio");
        button.setAttribute("aria-label", `${rating} sao`);
        button.dataset.rating = String(rating);

        const star = document.createElement("span");
        star.className = "star";
        star.setAttribute("aria-hidden", "true");
        button.appendChild(star);

        button.addEventListener("click", () => setPickerRating(rating));
        elements.ratingPicker.appendChild(button);
    }

    setPickerRating(initialRating);
}

function setPickerRating(rating) {
    const safeRating = Math.max(1, Math.min(5, Number(rating || 5)));
    const input = elements.reviewForm.querySelector("input[name='rating']");
    input.value = String(safeRating);

    const choices = Array.from(elements.ratingPicker.querySelectorAll(".star-choice"));
    choices.forEach((button, index) => {
        const current = index + 1;
        const checked = current === safeRating;
        button.setAttribute("aria-checked", checked ? "true" : "false");

        const star = button.querySelector(".star");
        star.classList.toggle("filled", current <= safeRating);
    });
}

function syncReviewFormState() {
    const canReview = Boolean(state.token) && Boolean(state.productModal.productId);
    const message = !state.token
        ? "Dang nhap role User de gui danh gia."
        : "Danh gia chi duoc phep khi ban da mua san pham (don COMPLETED).";

    elements.reviewForm.querySelector("button[type='submit']").disabled = !canReview;
    elements.reviewForm.querySelector("textarea[name='comment']").disabled = !canReview;
    elements.ratingPicker.querySelectorAll("button").forEach(button => {
        button.disabled = !canReview;
    });

    if (!canReview) {
        setReviewMessage(message);
    }
}

async function handleReviewSubmit(event) {
    event.preventDefault();
    if (!state.token) {
        setReviewMessage("Can dang nhap truoc khi gui danh gia.");
        return;
    }

    const productId = state.productModal.productId;
    if (!productId) {
        setReviewMessage("Khong tim thay san pham de danh gia.");
        return;
    }

    const form = new FormData(event.currentTarget);
    const rating = Number(form.get("rating") || 5);
    const comment = String(form.get("comment") || "").trim();

    if (!comment) {
        setReviewMessage("Vui long nhap comment.");
        return;
    }

    setReviewMessage("Dang gui danh gia...");

    try {
        const response = await fetch("/api/v1/reviews", {
            method: "POST",
            headers: authorizedJsonHeaders(),
            body: JSON.stringify({
                productId,
                rating,
                comment
            })
        });

        if (!response.ok) throw new Error(await readError(response, "Khong the gui danh gia."));

        event.currentTarget.reset();
        setPickerRating(5);
        setReviewMessage("Da gui danh gia. Cam on ban!");
        await loadProductReviews(productId);
    } catch (error) {
        setReviewMessage(error.message);
    }
}

function setReviewMessage(message) {
    elements.reviewMessage.textContent = message || "";
}

function formatDateTime(value) {
    const date = value ? new Date(value) : null;
    if (!date || Number.isNaN(date.getTime())) return "";

    return new Intl.DateTimeFormat("vi-VN", {
        dateStyle: "medium",
        timeStyle: "short"
    }).format(date);
}

function syncAuthUi() {
    const userLabel = state.user?.email || state.user?.Email || "Khach truy cap";
    elements.sessionState.textContent = state.token ? "Da dang nhap" : "Khach truy cap";
    elements.sessionDetail.textContent = state.token
        ? `Session dang hoat dong cho ${userLabel}. API co the goi cart va orders.`
        : "Dang xem catalog cong khai. Dang nhap de them vao gio va checkout.";
    elements.openAuthButton.textContent = state.token ? "Tai khoan" : "Dang nhap";
    elements.authMessage.textContent = state.token
        ? `Token da luu trong trinh duyet. User: ${userLabel}`
        : "Chua co token. Mot so tinh nang se chi doc.";
    renderCart();
    renderOrders();
    syncReviewFormState();
}

function logout() {
    state.token = "";
    state.user = null;
    state.cart = null;
    state.orders = [];
    localStorage.removeItem("affiliate_token");
    localStorage.removeItem("affiliate_user");
    syncAuthUi();
    setCheckoutMessage("Da dang xuat khoi phien lam viec.");
}

function persistSession() {
    localStorage.setItem("affiliate_token", state.token);
    localStorage.setItem("affiliate_user", JSON.stringify(state.user || null));
}

function authorizedHeaders() {
    return { Authorization: `Bearer ${state.token}` };
}

function authorizedJsonHeaders() {
    return { ...authorizedHeaders(), "Content-Type": "application/json" };
}

async function readError(response, fallback) {
    try {
        const payload = await response.json();
        return payload.message || payload.title || fallback;
    } catch {
        return fallback;
    }
}

function setCatalogMessage(message) {
    elements.catalogMessage.textContent = message;
}

function setAuthMessage(message) {
    elements.authMessage.textContent = message;
}

function setCheckoutMessage(message) {
    elements.checkoutMessage.textContent = message;
}

function formatCurrency(value) {
    return new Intl.NumberFormat("vi-VN", {
        style: "currency",
        currency: "VND",
        maximumFractionDigits: 0
    }).format(Number(value || 0));
}

function readStoredJson(key) {
    const raw = localStorage.getItem(key);
    if (!raw) return null;

    try {
        return JSON.parse(raw);
    } catch {
        return null;
    }
}

function escapeHtml(text) {
    return String(text)
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll('"', "&quot;")
        .replaceAll("'", "&#39;");
}
