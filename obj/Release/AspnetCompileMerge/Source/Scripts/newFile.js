// Initialize Stripe with your publishable key
document.addEventListener('DOMContentLoaded', (event) => {
    // Your Stripe.js code here
    // Create a map to store the prices and quantities of each product
    const productPrices = new Map();
    const productQuantities = new Map();
    productPrices.set('BalanceTest', 195);
    //productPrices.set('Basic', 25);
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
                <button class="decrease"title="Odobra ks">-</button>
                <span class="quantity" title="Poèet daného tovaru v ks">${quantity}</span>
                <button class="increase" title="Prida ks">+</button>
                <button class="remove"title="Odstráò položku">x</button>
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
