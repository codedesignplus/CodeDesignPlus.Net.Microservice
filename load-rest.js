import { check, sleep } from 'k6';
import http from 'k6/http';
import { uuidv4, randomIntBetween } from 'https://jslib.k6.io/k6-utils/1.4.0/index.js';


const params = {
    headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6Ilg1ZVhrNHh5b2pORnVtMWtsMll0djhkbE5QNC1jNTdkTzZRR1RWQndhTmsiLCJ0eXAiOiJKV1QifQ.eyJhdWQiOiJhNzRjYjE5Mi01OThjLTQ3NTctOTVhZS1iMzE1NzkzYmJiY2EiLCJpc3MiOiJodHRwczovL2NvZGVkZXNpZ25wbHVzLmIyY2xvZ2luLmNvbS8zNDYxZTMxMS1hNjZlLTQ2YWItYWZkZi0yYmJmYjcyYTVjYjAvdjIuMC8iLCJleHAiOjE3MjQ5MDYzMzIsIm5iZiI6MTcyNDkwMjczMiwib2lkIjoiMTZjOGIyYjctZjEwMi00YmZiLTlmYzctYzBmOGQyMzI0ZDE4Iiwic3ViIjoiMTZjOGIyYjctZjEwMi00YmZiLTlmYzctYzBmOGQyMzI0ZDE4IiwiZmFtaWx5X25hbWUiOiJEZXNpZ24gUGx1cyIsImNpdHkiOiJCb2dvdGEiLCJsZWdhbEFnZUdyb3VwQ2xhc3NpZmljYXRpb24iOiJBZHVsdCIsInBvc3RhbENvZGUiOiIxMTE2MTEiLCJzdHJlZXRBZGRyZXNzIjoiQ2FsbGUgM2EgIyA1M2MtMTMiLCJzdGF0ZSI6IkJvZ290YSIsImdpdmVuX25hbWUiOiJDb2RlIiwibmFtZSI6IkNvZGVEZXNpZ25QbHVzIiwiY291bnRyeSI6IkNvbG9tYmlhIiwiam9iVGl0bGUiOiJBcmNoaXRlY3QiLCJlbWFpbHMiOlsid2xpc2Nhbm85M0BnbWFpbC5jb20iLCJjb2RlZGVzaWducGx1c0BvdXRsb29rLmNvbSJdLCJ0ZnAiOiJCMkNfMV9Db2RlRGVzZWlnblBsdXMiLCJzY3AiOiJyZWFkIiwiYXpwIjoiYTc0Y2IxOTItNTk4Yy00NzU3LTk1YWUtYjMxNTc5M2JiYmNhIiwidmVyIjoiMS4wIiwiaWF0IjoxNzI0OTAyNzMyfQ.TSIri-A4eRKaqCKOd4zJ_LqFv7d2MQpUI2nm-ieuoHZDJMiWufVL1z5DQs7-6ta0Ch_OSODDKW80FZOpU-ehJ77yVBouRiEKdtqJpy4-rthlJASq0nz0qVZ71rwJKrZSh55fboZBxUW0qqDdRifXZfGvtxiP1HZnewiszklqeyelC51Mmw8gV7tANtw2VihuC9Jj0Qf7_7wDEGALkJ-IQ9Bxx7MmQqRnPppZNF-08w46rgEoe52aHeVthZXW6n3gnCVdnCZcRhzw0QHg8kuOA7P80y8qQSlqIQXCny0hOKyNULde6p0WX37bPdHi_j0sk7eB9yp5SwwcU8tqzFXbsg'
    },
};

export const options = {
    discardResponseBodies: true,
    scenarios: {
        contacts: {
            executor: 'constant-arrival-rate',
            duration: '600s',

            // How many iterations per timeUnit
            rate: 50,

            // Start `rate` iterations per second
            timeUnit: '1s',

            // Pre-allocate 2 VUs before starting the test
            preAllocatedVUs: 3,

            // Spin up a maximum of 50 VUs to sustain the defined
            // constant arrival rate.
            maxVUs: 5000,
        },
    },
};

export default async function () {

    const order = JSON.stringify({
        "id": uuidv4(),
        "client": {
            "id": uuidv4(),
            "name": `Client ${randomIntBetween(1, 1000)}`,
        }
    });

    let response = await http.asyncRequest('POST', 'http://localhost:5175/api/orders', order, params);

    check(response, {
        'is status 200': (r) => r.status === 200,
    });

    sleep(1);
}