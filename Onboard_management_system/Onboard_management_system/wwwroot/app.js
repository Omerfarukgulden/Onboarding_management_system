// Backend adresini buradan değiştirebilirsin (launchSettings.json -> http profili)
const API_BASE = "http://localhost:5010/api";

async function apiRequest(path, { method = "GET", body, auth = true } = {}) {
    const headers = { "Content-Type": "application/json" };

    if (auth) {
        const token = getToken();
        if (token) headers["Authorization"] = "Bearer " + token;
    }

    let res;
    try {
        res = await fetch(API_BASE + path, {
            method,
            headers,
            body: body !== undefined ? JSON.stringify(body) : undefined,
        });
    } catch (e) {
        throw new Error(
            "API'ye ulaşılamadı. Backend çalışıyor mu ve CORS ayarı yapıldı mı? (" + API_BASE + ")"
        );
    }

    if (res.status === 401) {
        logout();
        window.location.href = "login.html";
        throw new Error("Oturum sona erdi, tekrar giriş yapın.");
    }

    if (!res.ok) {
        let message = "İşlem başarısız (HTTP " + res.status + ")";
        try {
            const data = await res.clone().json();
            message = data.message || data.title || (typeof data === "string" ? data : message);
        } catch (e) {
            try {
                const text = await res.text();
                if (text) message = text;
            } catch (e2) {}
        }
        throw new Error(message);
    }

    if (res.status === 204) return null;
    const text = await res.text();
    return text ? JSON.parse(text) : null;
}