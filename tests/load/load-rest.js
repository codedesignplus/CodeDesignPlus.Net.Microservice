import { check, sleep } from 'k6';
import http from 'k6/http';
import { uuidv4, randomIntBetween } from 'https://jslib.k6.io/k6-utils/1.4.0/index.js';


const params = {
    headers: {
        'Content-Type': 'application/json',
        'X-Tenant': uuidv4(),
        'Authorization': 'Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6Ilg1ZVhrNHh5b2pORnVtMWtsMll0djhkbE5QNC1jNTdkTzZRR1RWQndhTmsiLCJ0eXAiOiJKV1QifQ.eyJhdWQiOiJhNzRjYjE5Mi01OThjLTQ3NTctOTVhZS1iMzE1NzkzYmJiY2EiLCJpc3MiOiJodHRwczovL2NvZGVkZXNpZ25wbHVzLmIyY2xvZ2luLmNvbS8zNDYxZTMxMS1hNjZlLTQ2YWItYWZkZi0yYmJmYjcyYTVjYjAvdjIuMC8iLCJleHAiOjE3MzYzMDg1ODIsIm5iZiI6MTczNjMwNDk4Miwib2lkIjoiYzAzZjI4ZGEtMTI4Yy00Yzk3LThjZTgtMDAzNmFkY2U1YmU1Iiwic3ViIjoiYzAzZjI4ZGEtMTI4Yy00Yzk3LThjZTgtMDAzNmFkY2U1YmU1IiwiZmFtaWx5X25hbWUiOiJMaXNjYW5vIiwiY2l0eSI6IkJvZ290w6EiLCJwb3N0YWxDb2RlIjoiMTExNjExIiwic3RyZWV0QWRkcmVzcyI6IkNhbGxlIDNhICMgNTNjLTEzIiwic3RhdGUiOiJCb2dvdGEiLCJnaXZlbl9uYW1lIjoiV2lsem9uIiwibmFtZSI6IldpbHpvbiBMaXNjYW5vIiwiY291bnRyeSI6IkNvbG9tYmlhIiwiam9iVGl0bGUiOiJBcmNoaXRlY2giLCJlbWFpbHMiOlsiY29kZWRlc2lnbnBsdXNAb3V0bG9vay5jb20iXSwidGZwIjoiQjJDXzFfQ29kZURlc2VpZ25QbHVzIiwic2NwIjoicmVhZCIsImF6cCI6ImE3NGNiMTkyLTU5OGMtNDc1Ny05NWFlLWIzMTU3OTNiYmJjYSIsInZlciI6IjEuMCIsImlhdCI6MTczNjMwNDk4Mn0.sI-xBgZEBL_1wlbGvXndYPljtL--4qw6225cUHqgR_ZDpbaiuok55wSRtI9t95mh105DwjMQLBIykyoT-Pn0VEyAvGproeBExiLNE49bM0-yIRQQs_4Bkt1hQAcgOizMRMOzeMVp_cNkxNoDKzinl8939deV8WXbr-HP2hQRzn9eY_odbhfyBKl5EclUTfePXhZszsn8lYs5oxpdWSyD5MvBrXySu-0bV2Q9Tmi6NNfcnRLZ36qQPoQfoSt0ETBFOj1iICpp4767xb5Zd4b4bVkKcuXB1F0sBLNKjDRb-yeRxVNepUDrLflb2zymSQw6u8dZXxnOWSspGmnejpXg6A'
    },
};

export const options = {
    discardResponseBodies: true,
    scenarios: {
        contacts: {
            executor: 'constant-arrival-rate',
            duration: '600s',

            // How many iterations per timeUnit
            rate: 10,

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
            "document": `${randomIntBetween(1, 2123123)}`,
            "typeDocument": "CC"
        },
        "address": {
          "country": "Colombia",
          "state": "Bogotá",
          "city": "Bogotá",
          "address": "Calle siempre viva",
          "codePostal": 111611
        }
    });

    let response = await http.asyncRequest('POST', 'http://localhost:5175/api/orders', order, params);

    check(response, {
        'is status 204': (r) => r.status === 204,
    });

    sleep(1);
}