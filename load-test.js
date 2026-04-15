import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
    stages: [
        { duration: '30s', target: 50 },   // زيادة تدريجية إلى 50 مستخدم
        { duration: '1m', target: 100 },   // ثبات عند 100 مستخدم
        { duration: '10s', target: 0 },    // إنزال تدريجي
    ],
};

const BASE_URL = 'http://localhost:5085';

export function setup() {
    // تسجيل الدخول مرة واحدة والحصول على التوكن (setup phase)
    const loginRes = http.post(`${BASE_URL}/api/Auth/login`, JSON.stringify({
        username: 'admin',
        password: '123456'
    }), { headers: { 'Content-Type': 'application/json' } });
    check(loginRes, { 'login succeeded': (r) => r.status === 200 });
    const token = loginRes.json('token');
    return { token };
}

export default function (data) {
    const token = data.token;

    // إنشاء عميل جديد
    const customerPayload = JSON.stringify({
        name: `عميل ${__VU}-${__ITER}`,
        phone: '0100000000',
        email: `customer${__VU}${__ITER}@test.com`,
        address: 'القاهرة'
    });
    const customerRes = http.post(`${BASE_URL}/api/Customers`, customerPayload, {
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        }
    });
    check(customerRes, { 'customer created': (r) => r.status === 201 });

    // الحصول على قائمة العملاء
    const getCustomers = http.get(`${BASE_URL}/api/Customers`, {
        headers: { 'Authorization': `Bearer ${token}` }
    });
    check(getCustomers, { 'customers fetched': (r) => r.status === 200 });

    // تقرير PDF
    const pdfRes = http.get(`${BASE_URL}/api/Reports/profit-loss/pdf?from=2026-01-01&to=2026-12-31`, {
        headers: { 'Authorization': `Bearer ${token}` }
    });
    check(pdfRes, { 'pdf generated': (r) => r.status === 200 });

    sleep(1);
}