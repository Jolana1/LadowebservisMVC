// Assumes you have Stripe.js loaded and a publishable key set in your _Layout.cshtml

const stripe = Stripe("pk_live_51P80kcHrPMzQ1ua8JUHSe4iUQ9sLHonQMmFzwyRKnq2xTpB6mhuJVc4OdBKa04BJzpsjjliSrBoNnftkBxwntFF300mePdWSx3");

let checkout;
initialize();

const validateEmail = async (email) => {
    const updateResult = await checkout.updateEmail(email);
    const isValid = updateResult.type !== "error";

    return { isValid, message: !isValid ? updateResult.error.message : null };
};

document
    .querySelector("#payment-form")
    .addEventListener("submit", handleSubmit);

// Fetches a Checkout Session and captures the client secret
async function initialize() {
    const promise = fetch("/create-checkout-session", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
    })
        .then((r) => r.json())
        .then((r) => r.clientSecret);

    const appearance = {
        theme: 'stripe',
    };
    checkout = await stripe.initCheckout({
        fetchClientSecret: () => promise,
        elementsOptions: { appearance },
    });

    // Compute quoted total from local cart + productCatalog (safe fallback)
    let quotedTotalText = 'Pay now';
    try {
        const rawCart = localStorage.getItem('cart');
        const cart = rawCart ? JSON.parse(rawCart) : {};
        const catalog = window.productCatalog || {};
        let total = 0;
        for (const pid of Object.keys(cart)) {
            const qty = Number(cart[pid]) || 0;
            const price = (catalog[pid] && Number(catalog[pid].price)) || 0;
            total += price * qty;
        }
        if (total > 0) {
            // format as EUR
            quotedTotalText = `Pay €${total.toFixed(2)} now`;
        }
    } catch (err) {
        console.warn('Failed to compute quoted total', err);
    }

    document.querySelector("#button-text").textContent = quotedTotalText;

    const emailInput = document.getElementById("email");
    const emailErrors = document.getElementById("email-errors");

    emailInput.addEventListener("input", () => {
        // Clear any validation errors
        emailErrors.textContent = "";
    });

    emailInput.addEventListener("blur", async () => {
        const newEmail = emailInput.value;
        if (!newEmail) {
            return;
        }

        const { isValid, message } = await validateEmail(newEmail);
        if (!isValid) {
            emailErrors.textContent = message;
        }
    });

    const paymentElement = checkout.createPaymentElement();
    paymentElement.mount("#payment-element");
    const billingAddressElement = checkout.createBillingAddressElement();
    billingAddressElement.mount("#billing-address-element");
}

async function handleSubmit(e) {
    e.preventDefault();
    setLoading(true);

    const email = document.getElementById("email").value;
    const { isValid, message } = await validateEmail(email);
    if (!isValid) {
        showMessage(message);
        setLoading(false);
        return;
    }

    const { error } = await checkout.confirm();

    // This point will only be reached if there is an immediate error when
    // confirming the payment. Otherwise, your customer will be redirected to
    // your `return_url`. For some payment methods like iDEAL, your customer will
    // be redirected to an intermediate site first to authorize the payment, then
    // redirected to the `return_url`.
    showMessage(error.message);

    setLoading(false);
}

// ------- UI helpers -------

function showMessage(messageText) {
    const messageContainer = document.querySelector("#payment-message");

    messageContainer.classList.remove("hidden");
    messageContainer.textContent = messageText;

    setTimeout(function () {
        messageContainer.classList.add("hidden");
        messageContainer.textContent = "";
    }, 4000);
}

// Show a spinner on payment submission
function setLoading(isLoading) {
    if (isLoading) {
        // Disable the button and show a spinner
        document.querySelector("#submit").disabled = true;
        document.querySelector("#spinner").classList.remove("hidden");
        document.querySelector("#button-text").classList.add("hidden");
    } else {
        document.querySelector("#submit").disabled = false;
        document.querySelector("#spinner").classList.add("hidden");
        document.querySelector("#button-text").classList.remove("hidden");
    }
}

// checkout.js

// Initialize Stripe with your publishable key
var stripe = Stripe('pk_live_51P80kcHrPMzQ1ua8JUHSe4iUQ9sLHonQMmFzwyRKnq2xTpB6mhuJVc4OdBKa04BJzpsjjliSrBoNnftkBxwntFF300mePdWSx3');

$('#checkout-btn').on('click', function () {
    // Get the first product in the cart (or let user select)
    $.get('/Server/GetCart', function (cart) {
        if (cart && cart.length > 0) {
            var productId = cart[0].Id; // Or let user pick
            $.post('/Server/CreateCheckoutSession', { productId: productId }, function (res) {
                if (res.success && res.sessionId) {
                    stripe.redirectToCheckout({ sessionId: res.sessionId });
                } else {
                    $('#payment-message').text(res.message || 'Payment failed.');
                }
            });
        }
    });
});




