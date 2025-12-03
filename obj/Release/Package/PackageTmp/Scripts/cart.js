(function () {
    //'use strict';

    //// Simple product price list (fallback only)
    //const productPrices = new Map([
    //    ['BalanceTest', 195.00],
    //    ['BalanceOil', 55.00],
    //    ['Zinobiotic', 33.00],
    //    ['ZinzinoXtend', 34.00]
    //]);

    const STORAGE_KEY = 'lws_cart_v1';

    function loadCart() {
        try {
            const json = localStorage.getItem(STORAGE_KEY);
            return json ? JSON.parse(json) : {};
        } catch (e) {
            return {};
        }
    }

    function saveCart(cart) {
        try {
            localStorage.setItem(STORAGE_KEY, JSON.stringify(cart));
        } catch (e) {
            // ignore storage errors
        }
    }

    function getCatalog() {
        try {
            return window.__lws_products || {};
        } catch (e) {
            return {};
        }
    }

    // compute subtotal, total quantity, discount (10% of subtotal applied to all products), and total
    function getCartTotals(cart) {
        const catalog = getCatalog();
        let subtotal = 0;
        let totalQty = 0;
        Object.keys(cart).forEach(key => {
            const item = cart[key];
            const q = item.quantity || 0;
            totalQty += q;
            let price = 0;
            if (catalog && catalog[key] && catalog[key].Price) {
                price = parseFloat(catalog[key].Price) || 0;
            } else if (catalog) {
                for (var k in catalog) {
                    if (!catalog.hasOwnProperty(k)) continue;
                    var val = catalog[k];
                    if (val && val.Name === key && val.Price) { price = parseFloat(val.Price) || 0; break; }
                }
            }
            if (!price) price = productPrices.get(key) || 0;
            subtotal += price * q;
        });

        // Apply 10% discount to the whole cart (per-product discount effectively)
        const discount = subtotal * 0.10;
        const total = subtotal - discount;
        return { subtotal: subtotal, totalQty: totalQty, discount: discount, total: total };
    }

    function renderCart() {
        const cart = loadCart();
        const listEl = document.querySelector('#cart-items-list');
        const itemsCountEl = document.querySelector('#cart-items');
        const menuCountEl = document.querySelector('#menu-cart-count');
        const totalAmountEl = document.querySelector('#total-amount');
        const totalDiscountEl = document.querySelector('#total-discount');

        if (!listEl || !itemsCountEl || !totalAmountEl) return;

        listEl.innerHTML = '';

        let totalCount = 0;
        const catalog = getCatalog();

        Object.keys(cart).forEach(key => {
            const item = cart[key];
            totalCount += item.quantity || 0;

            const li = document.createElement('li');
            li.className = 'list-group-item d-flex align-items-center ' + key;
            li.style.cursor = 'pointer';

            const img = document.createElement('img');
            img.src = item.image || ((catalog && catalog[key] && catalog[key].Image) ? catalog[key].Image : ('/Image/' + key + '.png'));
            img.alt = key;
            img.width = 48;
            img.height = 34;
            img.style.marginRight = '10px';

            var displayName = (catalog && catalog[key] && catalog[key].Name) ? catalog[key].Name : key;

            // unit price and discounted unit price
            var unitPrice = 0;
            if (catalog && catalog[key] && catalog[key].Price) unitPrice = parseFloat(catalog[key].Price) || 0;
            else unitPrice = productPrices.get(key) || 0;
            var discountedUnit = unitPrice * 0.9;

            const nameSpan = document.createElement('span');
            nameSpan.innerHTML = displayName + ' ';

            const qtySpan = document.createElement('span');
            qtySpan.className = 'quantity';
            qtySpan.textContent = String(item.quantity || 0);
            qtySpan.style.margin = '0 8px';

            const priceSpan = document.createElement('span');
            priceSpan.style.marginLeft = 'auto';
            priceSpan.style.fontWeight = '600';
            // show discounted line total, and optionally show original struck-through
            const lineTotal = (discountedUnit * (item.quantity || 0)).toFixed(2);
            priceSpan.innerHTML = '<span style="text-decoration:line-through;color:#999;margin-right:.4rem;">€' + ((unitPrice * (item.quantity || 0)).toFixed(2)) + '</span><span> €' + lineTotal + '</span>';

            // delete button for header mini-cart
            const delBtn = document.createElement('button');
            delBtn.type = 'button';
            delBtn.className = 'btn btn-xs btn-danger';
            delBtn.style.marginLeft = '8px';
            delBtn.textContent = 'x';
            delBtn.title = 'Odstrániť položku';
            delBtn.addEventListener('click', function (ev) {
                ev.stopPropagation();
                ev.preventDefault();
                const cartNow = loadCart();
                if (cartNow && cartNow[key]) { delete cartNow[key]; saveCart(cartNow); }
                renderCart();
                // also trigger cart page re-render if present
                if (window.lwsRenderCartPage) window.lwsRenderCartPage();
            });

            li.appendChild(img);
            li.appendChild(nameSpan);
            li.appendChild(qtySpan);
            li.appendChild(priceSpan);
            li.appendChild(delBtn);

            // clicking on the item navigates to the cart page for full checkout
            li.addEventListener('click', function () {
                window.location.href = '/Home/Kosik';
            });

            listEl.appendChild(li);
        });

        itemsCountEl.textContent = String(totalCount);
        if (menuCountEl) menuCountEl.textContent = String(totalCount);

        const totals = getCartTotals(cart);
        totalAmountEl.textContent = totals.total.toFixed(2);
        if (totalDiscountEl) {
            if (totals.discount > 0) totalDiscountEl.textContent = '(Zľava 10%: -€' + totals.discount.toFixed(2) + ')';
            else totalDiscountEl.textContent = '';
        }
    }

    function animateFlyToCart(imgSrc, startRect) {
        try {
            const cartTarget = document.querySelector('#menu-cart-count') || document.querySelector('#cart-items');
            if (!cartTarget) return;

            const fly = document.createElement('img');
            fly.src = imgSrc;
            fly.className = 'lws-fly-img';
            document.body.appendChild(fly);

            // set initial position
            fly.style.position = 'absolute';
            fly.style.left = startRect.left + 'px';
            fly.style.top = startRect.top + 'px';
            fly.style.width = startRect.width + 'px';
            fly.style.height = startRect.height + 'px';
            fly.style.transition = 'transform 0.8s ease-in-out, opacity 0.8s ease-in-out';
            fly.style.zIndex = 9999;

            // compute target center
            const targetRect = cartTarget.getBoundingClientRect();
            const destX = targetRect.left + (targetRect.width / 2) - (startRect.width / 2);
            const destY = targetRect.top + (targetRect.height / 2) - (startRect.height / 2);
            const translateX = destX - startRect.left;
            const translateY = destY - startRect.top;

            // force layout then animate
            requestAnimationFrame(function () {
                fly.style.transform = 'translate(' + translateX + 'px, ' + translateY + 'px) scale(0.2)';
                fly.style.opacity = '0.4';
            });

            // cleanup after animation
            fly.addEventListener('transitionend', function () {
                if (fly && fly.parentNode) fly.parentNode.removeChild(fly);
            });
        } catch (e) {
            // ignore animation errors
        }
    }

    function addToCartById(id, sourceImgEl) {
        if (!id) return;
        const cart = loadCart();
        const catalog = getCatalog();
        const p = catalog[id];
        // if catalog doesn't have id, try to treat id as name
        const img = (p && p.Image) ? p.Image : (sourceImgEl && sourceImgEl.src) || ('/Image/' + id + '.png');
        if (!cart[id]) {
            cart[id] = { quantity: 0, image: img };
        }
        // allow quantity increments in mini-cart; keep behavior consistent with page logic
        cart[id].quantity = (cart[id].quantity || 0) + 1;
        if (img) cart[id].image = img;
        saveCart(cart);

        // trigger animation using source image bounding rect
        try {
            if (sourceImgEl && sourceImgEl.getBoundingClientRect) {
                const rect = sourceImgEl.getBoundingClientRect();
                const startRect = { left: rect.left, top: rect.top, width: rect.width, height: rect.height };
                animateFlyToCart(img, startRect);
            }
        } catch (e) { }

        renderCart();
        // Expose a global hook so Kosik page can re-render itself when header changes
        window.lwsRenderCartHeader = renderCart;
        // also notify cart page if present
        if (window.lwsRenderCartPage) window.lwsRenderCartPage();
    }

    document.addEventListener('DOMContentLoaded', function () {
        renderCart();

        document.querySelectorAll('.add-to-cart').forEach(function (btn) {
            btn.addEventListener('click', function (ev) {
                ev.preventDefault();
                // find parent .products section for data attributes
                var section = btn.closest('.products');
                var productId = btn.getAttribute('data-id');
                var imgEl = section ? section.querySelector('img') : null;

                // prefer id; if none, try resolve by name
                if (!productId && section) {
                    productId = section.getAttribute('data-product-id') || section.getAttribute('data-id');
                    if (!productId) {
                        var name = section.getAttribute('data-product-name');
                        if (name) {
                            var catalog = getCatalog();
                            for (var k in catalog) {
                                if (!catalog.hasOwnProperty(k)) continue;
                                if (catalog[k] && catalog[k].Name === name) { productId = k; break; }
                            }
                        }
                    }
                }

                addToCartById(productId, imgEl || null);
            });
        });

        // Checkout without Stripe: redirect to a review/checkout page (Kosik)
        var checkoutBtn = document.getElementById('checkout-btn');
        if (checkoutBtn) {
            checkoutBtn.addEventListener('click', function () {
                // The cart is stored in localStorage; server-side can read it later if implemented
                window.location.href = '/Home/Kosik';
            });
        }

        // Optional: clear cart button
        var clearBtn = document.getElementById('clear-cart-btn');
        if (clearBtn) {
            clearBtn.addEventListener('click', function () {
                localStorage.removeItem(STORAGE_KEY);
                renderCart();
                if (window.lwsRenderCartPage) window.lwsRenderCartPage();
            });
        }
    });

    // Expose render function for header and ensure cart page can set its own renderer
    window.lwsRenderCartHeader = renderCart;
    if (!window.lwsRenderCartPage) window.lwsRenderCartPage = null;

})();





























































































































































































































