document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('recaptchaForm');
    form.addEventListener('submit', handleSubmit);
});

function handleSubmit(event) {
    event.preventDefault();

    const recaptchaResponse = document.querySelector('.g-recaptcha-response').value;
    if (!recaptchaResponse) {
        document.getElementById('message').innerText = 'Please complete the reCAPTCHA.';
        return;
    }
    
    const formData = new FormData(document.getElementById('recaptchaForm'));
    formData.set('recaptchaResponse', recaptchaResponse);

    fetch('http://localhost:SERVER_PORT/api/v1/recaptcha/verify-recaptcha', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: new URLSearchParams(formData)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(data => document.getElementById('message').innerText = data)
        .catch(() => document.getElementById('message').innerText = 'An error occurred during verification.');
}