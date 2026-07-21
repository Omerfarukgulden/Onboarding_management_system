const AUTH_KEY = "onboard_auth";

function getAuth() {
    const raw = localStorage.getItem(AUTH_KEY);
    return raw ? JSON.parse(raw) : null;
}

function saveAuth(data) {
    localStorage.setItem(AUTH_KEY, JSON.stringify(data));
}

function getToken() {
    const a = getAuth();
    return a ? a.token : null;
}

function getRole() {
    const a = getAuth();
    return a ? a.role : null;
}

function getUserId() {
    const a = getAuth();
    return a ? a.userId : null;
}

function getUsername() {
    const a = getAuth();
    return a ? a.username : null;
}

function logout() {
    localStorage.removeItem(AUTH_KEY);
}

function requireAuth() {
    if (!getAuth()) {
        window.location.href = "login.html";
    }
}