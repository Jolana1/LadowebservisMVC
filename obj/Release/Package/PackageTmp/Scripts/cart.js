// Initialize Stripe with your publishable key
document.addEventListener('DOMContentLoaded', (event) => {
    // Your Stripe.js code here
    // Create a map to store the prices and quantities of each product
    const productPrices = new Map();
    const productQuantities = new Map();
    productPrices.set('BalanceTest', 195);
    productPrices.set('Basic', 30);
    //productPrices.set('Premium', 399);
    productPrices.set('BalanceOil', 55);
    productPrices.set('Zinobiotic', 33);
    productPrices.set('ZinzinoXtend', 34);

    // Handle the product quantity change
    function calculateTotalAmount() {
        let totalAmount = 0;
        for (let [productName, quantity] of productQuantities) {
            const productPrice = productPrices.get(productName);
            if (typeof productPrice === 'number' && typeof quantity === 'number') {
                totalAmount += productPrice * quantity;
            }
        }
        return totalAmount;
    }
    document.querySelectorAll('.add-to-cart').forEach(button => {
        button.addEventListener('click', (event) => {
            const productName = event.target.parentElement.getAttribute('data-product-name');
            const cartItemsList = document.querySelector('#cart-items-list');
            let quantity = productQuantities.get(productName) || 0;
            productQuantities.set(productName, ++quantity);

            let listItem = document.querySelector(`#cart-items-list .${productName}`);
            if (!listItem) {
                listItem = document.createElement('li');
                listItem.classList.add(productName);
                listItem.innerHTML = `
                <img src="./Image/${productName}.png" alt="${productName}" width="35" height="28">
                <span>${productName}</span>
                <button class="decrease"title="Odobrať ks">-</button>
                <span class="quantity" title="Počet daného tovaru v ks">${quantity}</span>
                <button class="increase" title="Pridať ks">+</button>
                <button class="remove"title="Odstráň položku">x</button>
                `;
                cartItemsList.appendChild(listItem);

                listItem.querySelector('.increase').addEventListener('click', () => {
                    productQuantities.set(productName, ++quantity);
                    listItem.querySelector('.quantity').textContent = quantity;
                    document.querySelector('#total-amount').textContent = calculateTotalAmount();
                });

                listItem.querySelector('.decrease').addEventListener('click', () => {
                    if (quantity > 0) {
                        productQuantities.set(productName, --quantity);
                        listItem.querySelector('.quantity').textContent = quantity;
                    }
                    document.querySelector('#total-amount').textContent = calculateTotalAmount();
                });

                listItem.querySelector('.remove').addEventListener('click', () => {
                    productQuantities.delete(productName);
                    listItem.remove();
                    document.querySelector('#total-amount').textContent = calculateTotalAmount();
                });
            } else {
                listItem.querySelector('.quantity').textContent = quantity;
            }

            document.querySelector('#cart-items').textContent = Array.from(productQuantities.values()).reduce((a, b) => a + b, 0);
            document.querySelector('#total-amount').textContent = calculateTotalAmount();
        });
    });
    async function listAllProducts() {
        const products = await stripe.products.list();

        return products;
    }


    // Stripe's examples are localized to specific languages, but if
    // you wish to have Elements automatically detect your user's locale,
    //use `locale: 'auto'` instead.
    locale: window.__exampleLocale;
});



// Handle the product quantity change
    function calculateTotalAmount() {
        let totalAmount = 0;
        for (let [productName, quantity] of productQuantities) {
            const productPrice = productPrices.get(productName);
            if (typeof productPrice === 'number' && typeof quantity === 'number') {
                totalAmount += productPrice * quantity;
            }
        }
        return totalAmount;
    }


   

    async function payWithCard(stripe, card, clientSecret) {
        const { error, paymentIntent } = await stripe.confirmCardPayment(clientSecret, {
            payment_method: { card },
        });

        if (error) {
            // Payment has failed
            console.log('Payment failed:', error);
            // Display error message to your customer
        } else {
            if (paymentIntent.status === 'succeeded') {
                // Payment has succeeded
                console.log('Payment succeeded:', paymentIntent.id);
                // Display success message to your customer
            }
        }
    }

    // Get the clientSecret from your server and call payWithCard
    var clientSecret = 'your-client-secret-from-server';
    var card = elements.create('card');
    payWithCard(stripe, card, clientSecret);

    // Create a PaymentIntent with the cart details
    var createPaymentIntent = function (items) {
        return fetch('/create-payment-intent', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                items: items,
            }),
        }).then(function (result) {
            return result.json();
        });
    };

    // Handle the payment process
    function processPayment() {
        stripe.redirectToCheckout({
            // Replace with the ID of your SKU
            items: [{ sku: 'pmc_1OgVzPFdjXAPBwqYvJ1Nr799', quantity: 1 }],
            successUrl: 'https://buy.stripe.com/4gweXJgm2caC2dy3cc/success',
            cancelUrl: 'https://buy.stripe.com/4gweXJgm2caC2dy3cc/canceled',
        }).then(function (result) {
            if (result.error) {
                // If redirectToCheckout fails due to a browser or network error, display the localized error message to your customer.
                var displayError = document.getElementById('error-message');
                displayError.textContent = result.error.message;
            }
        });
    }

// cart.js

$(document).ready(function () {
    // Fetch and render cart on page load
    fetchCart();

    // Remove item from cart
    $('#cart-list').on('click', '.remove-item', function () {
        var productName = $(this).data('id');
        $.post('/Server/RemoveFromCart', { productName: productId }, function (res) {
            if (res.success) {
                fetchCart();
            }
        });
    });
});

// Fetch cart items and update UI
function fetchCart() {
    $.get('/Server/GetCart', function (cart) {
        var $list = $('#cart-list');
        $list.empty();
        var total = 0;
        var count = 0;
        if (cart && cart.length > 0) {
            cart.forEach(function (item) {
                $list.append(
                    `<li class="list-group-item d-flex justify-content-between align-items-center">
                        ${productName} <span>€${item.Price.toFixed(2)}</span>
                        <button class="btn btn-danger btn-sm remove-item" data-id="${item.Id}">&times;</button>
                        <button class="abtn btn-primary btn-sm buy-item" data-id="${item.Id}">Buy</button>
                    </li>`
                );
                total += item.Price;
                count++;
            });
            $('#checkout-btn').prop('enabled', true);
        } else {
            $list.append('<li class="list-group-item">Cart is empty.</li>');
            $('#checkout-btn').prop('enabled', false);
        }
        $('#cart-count').text(count);
        $('#cart-total').text(total.toFixed(2));
        $('#checkout-btn').prop('enabled', cart.length === 0);
    });
}

// Handle buy button for individual products
$('#cart-items-list').on('click', '.buy-item', function () {
    var productId = $(this).data('id');
    startStripePayment(productName);
});





























































































































































































































