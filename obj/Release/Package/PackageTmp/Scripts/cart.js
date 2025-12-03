(function () {
    'use strict';

    // Simple product price list (fallback only)
    const productPrices = new Map([
        ['BalanceTest', 195.00],
        ['BalanceOil', 55.00],
        ['Zinobiotic', 33.00],
        ['ZinzinoXtend', 34.00]
    ]);

    const STORAGE_KEY = 'lws_cart_v1';
    const FAV_KEY = 'lws_fav_v1';

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

    function loadFavorites() {
        try {
            const j = localStorage.getItem(FAV_KEY);
            return j ? JSON.parse(j) : [];
        } catch (e) { return []; }
    }

    function saveFavorites(arr) {
        try { localStorage.setItem(FAV_KEY, JSON.stringify(arr || [])); } catch (e) { }
    }

    function isFavorite(id) {
        if (!id) return false;
        const fav = loadFavorites();
        return fav.indexOf(id) !== -1;
    }

    function toggleFavorite(id) {
        if (!id) return;
        const fav = loadFavorites();
        const idx = fav.indexOf(id);
        if (idx === -1) fav.push(id); else fav.splice(idx, 1);
        saveFavorites(fav);
        renderFavoriteButtons();
        // also update favorites page if open
        if (window.lwsRenderFavorites) window.lwsRenderFavorites();
    }

    function renderFavoriteButtons() {
        // buttons in product cards
        document.querySelectorAll('.product-heart').forEach(function (btn) {
            try {
                const id = btn.getAttribute('data-id') || btn.dataset.id;
                if (isFavorite(id)) {
                    btn.classList.add('favorited');
                    btn.innerHTML = '<i class="fa fa-heart" aria-hidden="true"></i>';
                } else {
                    btn.classList.remove('favorited');
                    btn.innerHTML = '<i class="fa fa-heart-o" aria-hidden="true"></i>';
                }
            } catch (e) { }
        });
        // buttons in mini-cart
        document.querySelectorAll('.product-heart-mini').forEach(function (btn) {
            try {
                const id = btn.getAttribute('data-id') || btn.dataset.id;
                if (isFavorite(id)) {
                    btn.classList.add('favorited');
                    btn.innerHTML = '<i class="fa fa-heart" aria-hidden="true"></i>';
                } else {
                    btn.classList.remove('favorited');
                    btn.innerHTML = '<i class="fa fa-heart-o" aria-hidden="true"></i>';
                }
            } catch (e) { }
        });
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
            if (!price) price = (productPrices && productPrices.get) ? productPrices.get(key) || 0 : 0;
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
            else unitPrice = (productPrices && productPrices.get) ? productPrices.get(key) || 0 : 0;
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

            // favorite button for header mini-cart
            const favBtn = document.createElement('button');
            favBtn.type = 'button';
            favBtn.className = 'product-heart-mini btn btn-link';
            favBtn.style.marginLeft = '8px';
            favBtn.setAttribute('data-id', key);
            favBtn.title = 'Pridať do obľúbených';
            favBtn.addEventListener('click', function (ev) {
                ev.stopPropagation(); ev.preventDefault();
                toggleFavorite(key);
            });

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
            li.appendChild(favBtn);
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

        // ensure favorite icons reflect current state
        renderFavoriteButtons();
        renderHeaderPaymentButton();
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

    function renderFavorites() {
        const listEl = document.querySelector('#favoritesList');
        if (!listEl) return;

        // clear existing content
        listEl.innerHTML = '';

        const favIds = loadFavorites();
        const catalog = getCatalog();

        favIds.forEach(id => {
            const item = catalog[id];
            if (!item) return; // skip if not found in catalog

            const li = document.createElement('li');
            li.className = 'list-group-item d-flex align-items-center ' + id;
            li.style.cursor = 'pointer';

            const img = document.createElement('img');
            img.src = item.Image || ('/Image/' + id + '.png');
            img.alt = id;
            img.width = 48;
            img.height = 34;
            img.style.marginRight = '10px';

            const nameSpan = document.createElement('span');
            nameSpan.innerHTML = item.Name + ' ';

            const priceSpan = document.createElement('span');
            priceSpan.style.marginLeft = 'auto';
            priceSpan.style.fontWeight = '600';
            // assume price is always available in catalog for favorites
            const price = parseFloat(item.Price) || 0;
            const discountedPrice = (price * 0.9).toFixed(2);
            priceSpan.innerHTML = '<span style="text-decoration:line-through;color:#999;margin-right:.4rem;">€' + (price.toFixed(2)) + '</span><span> €' + discountedPrice + '</span>';

            const addBtn = document.createElement('button');
            addBtn.type = 'button';
            addBtn.className = 'btn btn-sm btn-primary';
            addBtn.style.marginLeft = '8px';
            addBtn.textContent = 'Pridať do košíka';
            addBtn.addEventListener('click', function (ev) {
                ev.stopPropagation(); ev.preventDefault();
                addToCartById(id);
            });

            const delBtn = document.createElement('button');
            delBtn.type = 'button';
            delBtn.className = 'btn btn-sm btn-danger';
            delBtn.style.marginLeft = '4px';
            delBtn.textContent = 'Odstrániť';
            delBtn.addEventListener('click', function (ev) {
                ev.stopPropagation();
                ev.preventDefault();
                toggleFavorite(id);
                renderFavorites(); // re-render favorites list
            });

            li.appendChild(img);
            li.appendChild(nameSpan);
            li.appendChild(priceSpan);
            li.appendChild(addBtn);
            li.appendChild(delBtn);

            listEl.appendChild(li);
        });
    }

    function renderHeaderPaymentButton() {
        var headerWrap = document.querySelector('.cart-fluid');
        if (!headerWrap) return;

        var btnId = 'header-pay-btn';
        var existingBtn = document.getElementById(btnId);
        if (existingBtn) {
            // If button already exists, just return (avoid duplicates)
            return;
        }

        // Create new button element
        var payBtn = document.createElement('a');
        payBtn.id = btnId;
        payBtn.className = 'btn btn-danger btn-sm';
        payBtn.style.marginLeft = '16px';
        payBtn.textContent = 'Chodte na Prejsť k platbe';
        payBtn.href = '/Home/Kosik?checkout=1'; // point to Kosik with checkout=1
        payBtn.role = 'button';

        // Disable button if cart is empty
        const cart = loadCart();
        const isEmptyCart = !cart || Object.keys(cart).length === 0;
        if (isEmptyCart) {
            payBtn.classList.add('disabled');
            payBtn.setAttribute('aria-disabled', 'true');
        } else {
            payBtn.classList.remove('disabled');
            payBtn.removeAttribute('aria-disabled');
        }

        headerWrap.appendChild(payBtn);
    }

    document.addEventListener('DOMContentLoaded', function () {
        renderCart();
        renderFavoriteButtons();
        renderFavorites(); // also render favorites on load
        renderHeaderPaymentButton(); // ensure header payment button is rendered

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

        // Attach handler for favorite heart buttons
        document.querySelectorAll('.product-heart').forEach(function (btn) {
            btn.addEventListener('click', function (ev) {
                ev.preventDefault();
                const productId = btn.getAttribute('data-id');
                toggleFavorite(productId);
            });
        });
        // Re-attach handlers for mini-cart favorite buttons
        document.querySelectorAll('.product-heart-mini').forEach(function (btn) {
            btn.addEventListener('click', function (ev) {
                ev.preventDefault();
                const productId = btn.getAttribute('data-id');
                toggleFavorite(productId);
            });
        });

        // Auto-trigger Stripe payment if URL contains ?checkout=1
        if (window.location.search.includes('checkout=1')) {
            setTimeout(function () {
                // Try to find and click the Stripe buy button
                var stripeButton = document.querySelector('stripe-buy-button');
                if (stripeButton) {
                    stripeButton.click();
                } else {
                    // Fallback: try to find a button with class 'stripe-buy-button' or 'stripe-button'
                    var fallbackButton = document.querySelector('button.stripe-buy-button, button.stripe-button');
                    if (fallbackButton) {
                        fallbackButton.click();
                    }
                }
            }, 700); // delay to ensure buttons are rendered
        }
    });

    // Expose render function for header and ensure cart page can set its own renderer
    window.lwsRenderCartHeader = renderCart;
    window.lwsRenderFavorites = renderFavorites;
    if (!window.lwsRenderCartPage) window.lwsRenderCartPage = null;

})();

























































































































































































































































































































































































































































