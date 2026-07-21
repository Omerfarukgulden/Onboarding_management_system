requireAuth();

const ROLE_LABELS = { Admin: "Admin", Ik: "İK", DepartmentUser: "Departman Kullanıcısı" };

// Rol bazlı menü tanımı
const SECTIONS_BY_ROLE = {
    Admin: [
        { key: "departments", label: "Departmanlar" },
        { key: "positions", label: "Pozisyonlar" },
        { key: "users", label: "Kullanıcılar" },
    ],
    Ik: [
        { key: "employees", label: "Çalışanlar" },
        { key: "templates", label: "Onboarding Şablonları" },
        { key: "processes", label: "Onboarding Süreçleri" },
        { key: "tasks", label: "Görevler" },
    ],
    DepartmentUser: [
        { key: "tasks", label: "Görevler" },
    ],
};

const TASK_STATUS_OPTIONS = ["Pending", "InProgress", "Completed", "Cancelled"];
const TASK_STATUS_LABELS = {
    Pending: "Bekliyor",
    InProgress: "Devam Ediyor",
    Completed: "Tamamlandı",
    Cancelled: "İptal",
};
const PROCESS_STATUS_LABELS = {
    Preparing: "Hazırlanıyor",
    InProgress: "Devam Ediyor",
    Completed: "Tamamlandı",
    Cancelled: "İptal",
};

document.getElementById("logoutBtn").addEventListener("click", () => {
    logout();
    window.location.href = "login.html";
});

function esc(v) {
    if (v === null || v === undefined) return "";
    return String(v).replace(/[&<>"']/g, (c) => ({
        "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#39;",
    }[c]));
}

function fmtDate(d) {
    if (!d) return "";
    return new Date(d).toLocaleDateString("tr-TR");
}

function setContent(html) {
    document.getElementById("content").innerHTML = html;
}

function showError(msg) {
    const box = document.getElementById("actionMsg");
    if (box) {
        box.className = "error";
        box.textContent = msg;
    } else {
        alert(msg);
    }
}

function showInfo(msg) {
    const box = document.getElementById("actionMsg");
    if (box) {
        box.className = "info";
        box.textContent = msg;
    }
}

// ---------------- init ----------------
function init() {
    document.getElementById("userNameLabel").textContent = getUsername();
    document.getElementById("userRoleLabel").textContent = ROLE_LABELS[getRole()] || getRole();

    const sections = SECTIONS_BY_ROLE[getRole()] || [];
    const nav = document.getElementById("navMenu");
    nav.innerHTML = "";
    sections.forEach((s, i) => {
        const a = document.createElement("a");
        a.textContent = s.label;
        a.dataset.key = s.key;
        a.addEventListener("click", () => selectSection(s.key));
        nav.appendChild(a);
    });

    if (sections.length > 0) selectSection(sections[0].key);
    else setContent("<p>Bu kullanıcı için tanımlı bir bölüm yok.</p>");
}

function selectSection(key) {
    document.querySelectorAll("#navMenu a").forEach((a) => {
        a.classList.toggle("active", a.dataset.key === key);
    });

    const renderers = {
        departments: renderDepartments,
        positions: renderPositions,
        users: renderUsers,
        employees: renderEmployees,
        templates: renderTemplates,
        processes: renderProcesses,
        tasks: renderTasks,
    };

    const fn = renderers[key];
    if (fn) fn();
}

// ================= DEPARTMANLAR (Admin) =================
async function renderDepartments() {
    setContent(`
    <h2>Departmanlar</h2>
    <div class="error" id="actionMsg"></div>
    <div class="card">
      <h3 id="deptFormTitle">Yeni Departman</h3>
      <form id="deptForm" class="form-grid">
        <input type="hidden" id="deptId" />
        <div>
          <label>Ad</label>
          <input id="deptName" required />
        </div>
        <div>
          <label>Açıklama</label>
          <input id="deptDesc" />
        </div>
        <div id="deptActiveWrap" class="hidden">
          <label>Aktif mi?</label>
          <select id="deptActive">
            <option value="true">Evet</option>
            <option value="false">Hayır</option>
          </select>
        </div>
        <div class="form-actions">
          <button type="submit">Kaydet</button>
          <button type="button" class="secondary" id="deptCancel">Vazgeç</button>
        </div>
      </form>
    </div>
    <div class="card">
      <table>
        <thead><tr><th>Id</th><th>Ad</th><th>Açıklama</th><th>Aktif</th><th></th></tr></thead>
        <tbody id="deptTableBody"></tbody>
      </table>
    </div>
  `);

    document.getElementById("deptCancel").addEventListener("click", resetDeptForm);
    document.getElementById("deptForm").addEventListener("submit", saveDept);

    await loadDepartments();
}

async function loadDepartments() {
    try {
        const list = await apiRequest("/Departments");
        const body = document.getElementById("deptTableBody");
        body.innerHTML = list.map((d) => `
      <tr>
        <td>${d.id}</td>
        <td>${esc(d.name)}</td>
        <td>${esc(d.description)}</td>
        <td>${d.isActive ? "Evet" : "Hayır"}</td>
        <td class="row-actions">
          <button class="small" onclick="editDept(${d.id}, '${escJs(d.name)}', '${escJs(d.description)}', ${d.isActive})">Düzenle</button>
          <button class="small danger" onclick="deleteDept(${d.id})">Sil</button>
        </td>
      </tr>`).join("");
    } catch (e) {
        showError(e.message);
    }
}

function escJs(v) {
    return String(v || "").replace(/'/g, "\\'");
}

function editDept(id, name, desc, active) {
    document.getElementById("deptFormTitle").textContent = "Departman Düzenle";
    document.getElementById("deptId").value = id;
    document.getElementById("deptName").value = name;
    document.getElementById("deptDesc").value = desc;
    document.getElementById("deptActiveWrap").classList.remove("hidden");
    document.getElementById("deptActive").value = String(active);
}

function resetDeptForm() {
    document.getElementById("deptFormTitle").textContent = "Yeni Departman";
    document.getElementById("deptId").value = "";
    document.getElementById("deptForm").reset();
    document.getElementById("deptActiveWrap").classList.add("hidden");
}

async function saveDept(e) {
    e.preventDefault();
    const id = document.getElementById("deptId").value;
    const name = document.getElementById("deptName").value;
    const description = document.getElementById("deptDesc").value;

    try {
        if (id) {
            const isActive = document.getElementById("deptActive").value === "true";
            await apiRequest(`/Departments/${id}`, { method: "PUT", body: { Name: name, Description: description, IsActive: isActive } });
            showInfo("Departman güncellendi.");
        } else {
            await apiRequest("/Departments", { method: "POST", body: { Name: name, Description: description } });
            showInfo("Departman oluşturuldu.");
        }
        resetDeptForm();
        await loadDepartments();
    } catch (err) {
        showError(err.message);
    }
}

async function deleteDept(id) {
    if (!confirm("Bu departmanı silmek istediğine emin misin?")) return;
    try {
        await apiRequest(`/Departments/${id}`, { method: "DELETE" });
        await loadDepartments();
    } catch (e) {
        showError(e.message);
    }
}

// ================= POZİSYONLAR (Admin) =================
async function renderPositions() {
    setContent(`
    <h2>Pozisyonlar</h2>
    <div class="error" id="actionMsg"></div>
    <div class="card">
      <h3 id="posFormTitle">Yeni Pozisyon</h3>
      <form id="posForm" class="form-grid">
        <input type="hidden" id="posId" />
        <div><label>Ad</label><input id="posName" required /></div>
        <div><label>Açıklama</label><input id="posDesc" /></div>
        <div class="form-actions">
          <button type="submit">Kaydet</button>
          <button type="button" class="secondary" id="posCancel">Vazgeç</button>
        </div>
      </form>
    </div>
    <div class="card">
      <table>
        <thead><tr><th>Id</th><th>Ad</th><th>Açıklama</th><th></th></tr></thead>
        <tbody id="posTableBody"></tbody>
      </table>
    </div>
  `);

    document.getElementById("posCancel").addEventListener("click", resetPosForm);
    document.getElementById("posForm").addEventListener("submit", savePos);
    await loadPositions();
}

async function loadPositions() {
    try {
        const list = await apiRequest("/Positions");
        document.getElementById("posTableBody").innerHTML = list.map((p) => `
      <tr>
        <td>${p.id}</td>
        <td>${esc(p.name)}</td>
        <td>${esc(p.description)}</td>
        <td class="row-actions">
          <button class="small" onclick="editPos(${p.id}, '${escJs(p.name)}', '${escJs(p.description)}')">Düzenle</button>
          <button class="small danger" onclick="deletePos(${p.id})">Sil</button>
        </td>
      </tr>`).join("");
    } catch (e) { showError(e.message); }
}

function editPos(id, name, desc) {
    document.getElementById("posFormTitle").textContent = "Pozisyon Düzenle";
    document.getElementById("posId").value = id;
    document.getElementById("posName").value = name;
    document.getElementById("posDesc").value = desc;
}

function resetPosForm() {
    document.getElementById("posFormTitle").textContent = "Yeni Pozisyon";
    document.getElementById("posId").value = "";
    document.getElementById("posForm").reset();
}

async function savePos(e) {
    e.preventDefault();
    const id = document.getElementById("posId").value;
    const name = document.getElementById("posName").value;
    const description = document.getElementById("posDesc").value;
    try {
        if (id) {
            await apiRequest(`/Positions/${id}`, { method: "PUT", body: { Name: name, Description: description } });
            showInfo("Pozisyon güncellendi.");
        } else {
            await apiRequest("/Positions", { method: "POST", body: { Name: name, Description: description } });
            showInfo("Pozisyon oluşturuldu.");
        }
        resetPosForm();
        await loadPositions();
    } catch (err) { showError(err.message); }
}

async function deletePos(id) {
    if (!confirm("Bu pozisyonu silmek istediğine emin misin?")) return;
    try {
        await apiRequest(`/Positions/${id}`, { method: "DELETE" });
        await loadPositions();
    } catch (e) { showError(e.message); }
}

// ================= KULLANICILAR (Admin) =================
async function renderUsers() {
    setContent(`
    <h2>Kullanıcılar</h2>
    <div class="error" id="actionMsg"></div>
    <div class="card">
      <h3 id="userFormTitle">Yeni Kullanıcı</h3>
      <form id="userForm" class="form-grid">
        <input type="hidden" id="userId" />
        <div><label>Kullanıcı Adı</label><input id="userUsername" required /></div>
        <div><label>Email</label><input id="userEmail" type="email" required /></div>
        <div id="userPasswordWrap"><label>Şifre</label><input id="userPassword" type="password" /></div>
        <div>
          <label>Rol</label>
          <select id="userRole">
            <option value="Admin">Admin</option>
            <option value="Ik">İK</option>
            <option value="DepartmentUser">Departman Kullanıcısı</option>
          </select>
        </div>
        <div><label>Departman Id</label><input id="userDeptId" type="number" /></div>
        <div id="userActiveWrap" class="hidden">
          <label>Aktif mi?</label>
          <select id="userActive"><option value="true">Evet</option><option value="false">Hayır</option></select>
        </div>
        <div class="form-actions">
          <button type="submit">Kaydet</button>
          <button type="button" class="secondary" id="userCancel">Vazgeç</button>
        </div>
      </form>
    </div>
    <div class="card">
      <table>
        <thead><tr><th>Id</th><th>Kullanıcı Adı</th><th>Email</th><th>Rol</th><th>Departman</th><th>Aktif</th><th></th></tr></thead>
        <tbody id="userTableBody"></tbody>
      </table>
    </div>
  `);

    document.getElementById("userCancel").addEventListener("click", resetUserForm);
    document.getElementById("userForm").addEventListener("submit", saveUser);
    await loadUsers();
}

async function loadUsers() {
    try {
        const list = await apiRequest("/Users");
        document.getElementById("userTableBody").innerHTML = list.map((u) => `
      <tr>
        <td>${u.id}</td>
        <td>${esc(u.username)}</td>
        <td>${esc(u.email)}</td>
        <td>${ROLE_LABELS[u.role] || u.role}</td>
        <td>${esc(u.departmentName) || u.departmentId || "-"}</td>
        <td>${u.isActive ? "Evet" : "Hayır"}</td>
        <td class="row-actions">
          <button class="small" onclick='editUser(${u.id}, ${JSON.stringify(u.email)}, ${JSON.stringify(u.role)}, ${u.departmentId ?? "null"}, ${u.isActive})'>Düzenle</button>
          <button class="small danger" onclick="deleteUser(${u.id})">Sil</button>
        </td>
      </tr>`).join("");
    } catch (e) { showError(e.message); }
}

function editUser(id, email, role, deptId, active) {
    document.getElementById("userFormTitle").textContent = "Kullanıcı Düzenle (Id: " + id + ")";
    document.getElementById("userId").value = id;
    document.getElementById("userUsername").disabled = true;
    document.getElementById("userUsername").value = "";
    document.getElementById("userUsername").placeholder = "(değiştirilemez)";
    document.getElementById("userPasswordWrap").classList.add("hidden");
    document.getElementById("userEmail").value = email;
    document.getElementById("userRole").value = role;
    document.getElementById("userDeptId").value = deptId ?? "";
    document.getElementById("userActiveWrap").classList.remove("hidden");
    document.getElementById("userActive").value = String(active);
}

function resetUserForm() {
    document.getElementById("userFormTitle").textContent = "Yeni Kullanıcı";
    document.getElementById("userId").value = "";
    document.getElementById("userForm").reset();
    document.getElementById("userUsername").disabled = false;
    document.getElementById("userUsername").placeholder = "";
    document.getElementById("userPasswordWrap").classList.remove("hidden");
    document.getElementById("userActiveWrap").classList.add("hidden");
}

async function saveUser(e) {
    e.preventDefault();
    const id = document.getElementById("userId").value;
    const email = document.getElementById("userEmail").value;
    const role = document.getElementById("userRole").value;
    const deptRaw = document.getElementById("userDeptId").value;
    const departmentId = deptRaw ? parseInt(deptRaw) : null;

    try {
        if (id) {
            const isActive = document.getElementById("userActive").value === "true";
            await apiRequest(`/Users/${id}`, { method: "PUT", body: { Email: email, Role: role, DepartmentId: departmentId, IsActive: isActive } });
            showInfo("Kullanıcı güncellendi.");
        } else {
            const username = document.getElementById("userUsername").value;
            const password = document.getElementById("userPassword").value;
            await apiRequest("/Users", { method: "POST", body: { Username: username, Email: email, Password: password, Role: role, DepartmentId: departmentId } });
            showInfo("Kullanıcı oluşturuldu.");
        }
        resetUserForm();
        await loadUsers();
    } catch (err) { showError(err.message); }
}

async function deleteUser(id) {
    if (!confirm("Bu kullanıcıyı silmek istediğine emin misin?")) return;
    try {
        await apiRequest(`/Users/${id}`, { method: "DELETE" });
        await loadUsers();
    } catch (e) { showError(e.message); }
}

// ================= ÇALIŞANLAR (Ik) =================
async function renderEmployees() {
    setContent(`
    <h2>Çalışanlar</h2>
    <div class="error" id="actionMsg"></div>
    <div class="card">
      <h3 id="empFormTitle">Yeni Çalışan</h3>
      <form id="empForm" class="form-grid">
        <input type="hidden" id="empId" />
        <div><label>Ad</label><input id="empName" required /></div>
        <div><label>Soyad</label><input id="empSurname" required /></div>
        <div><label>Email</label><input id="empEmail" type="email" required /></div>
        <div><label>Telefon</label><input id="empPhone" required /></div>
        <div><label>Adres</label><input id="empAddress" required /></div>
        <div><label>Kan Grubu</label><input id="empBlood" required /></div>
        <div><label>Cinsiyet</label><input id="empGender" required /></div>
        <div><label>İşe Giriş Tarihi</label><input id="empHireDate" type="date" required /></div>
        <div id="empEndDateWrap" class="hidden"><label>Çıkış Tarihi</label><input id="empEndDate" type="date" /></div>
        <div><label>Departman Id</label><input id="empDeptId" type="number" required /></div>
        <div><label>Pozisyon Id</label><input id="empPosId" type="number" required /></div>
        <div class="form-actions">
          <button type="submit">Kaydet</button>
          <button type="button" class="secondary" id="empCancel">Vazgeç</button>
        </div>
      </form>
      <p class="muted">Not: Departman/Pozisyon Id değerlerini Admin panelinden öğrenebilirsin.</p>
    </div>
    <div class="card">
      <table>
        <thead><tr><th>Id</th><th>Ad Soyad</th><th>Email</th><th>Telefon</th><th>Departman</th><th>Pozisyon</th><th>İşe Giriş</th><th></th></tr></thead>
        <tbody id="empTableBody"></tbody>
      </table>
    </div>
  `);

    document.getElementById("empCancel").addEventListener("click", resetEmpForm);
    document.getElementById("empForm").addEventListener("submit", saveEmp);
    await loadEmployees();
}

async function loadEmployees() {
    try {
        const list = await apiRequest("/Employees");
        document.getElementById("empTableBody").innerHTML = list.map((emp) => `
      <tr>
        <td>${emp.empId}</td>
        <td>${esc(emp.empName)} ${esc(emp.empSurname)}</td>
        <td>${esc(emp.empEmail)}</td>
        <td>${esc(emp.empPhone)}</td>
        <td>${esc(emp.departmentName) || emp.departmentId}</td>
        <td>${esc(emp.positionName) || emp.positionId}</td>
        <td>${fmtDate(emp.hireDate)}</td>
        <td class="row-actions">
          <button class="small" onclick='editEmp(${JSON.stringify(emp)})'>Düzenle</button>
          <button class="small danger" onclick="deleteEmp(${emp.empId})">Sil</button>
        </td>
      </tr>`).join("");
    } catch (e) { showError(e.message); }
}

function toDateInput(v) {
    if (!v) return "";
    return new Date(v).toISOString().slice(0, 10);
}

function editEmp(emp) {
    document.getElementById("empFormTitle").textContent = "Çalışan Düzenle (Id: " + emp.empId + ")";
    document.getElementById("empId").value = emp.empId;
    document.getElementById("empName").value = emp.empName;
    document.getElementById("empSurname").value = emp.empSurname;
    document.getElementById("empEmail").value = emp.empEmail;
    document.getElementById("empPhone").value = emp.empPhone;
    document.getElementById("empAddress").value = emp.empAddress;
    document.getElementById("empBlood").value = emp.empBlood;
    document.getElementById("empGender").value = emp.empGender;
    document.getElementById("empHireDate").value = toDateInput(emp.hireDate);
    document.getElementById("empEndDateWrap").classList.remove("hidden");
    document.getElementById("empEndDate").value = toDateInput(emp.endDate);
    document.getElementById("empDeptId").value = emp.departmentId;
    document.getElementById("empPosId").value = emp.positionId;
}

function resetEmpForm() {
    document.getElementById("empFormTitle").textContent = "Yeni Çalışan";
    document.getElementById("empId").value = "";
    document.getElementById("empForm").reset();
    document.getElementById("empEndDateWrap").classList.add("hidden");
}

async function saveEmp(e) {
    e.preventDefault();
    const id = document.getElementById("empId").value;
    const body = {
        EmpName: document.getElementById("empName").value,
        EmpSurname: document.getElementById("empSurname").value,
        EmpEmail: document.getElementById("empEmail").value,
        EmpPhone: document.getElementById("empPhone").value,
        EmpAddress: document.getElementById("empAddress").value,
        EmpBlood: document.getElementById("empBlood").value,
        EmpGender: document.getElementById("empGender").value,
        HireDate: document.getElementById("empHireDate").value,
        DepartmentId: parseInt(document.getElementById("empDeptId").value),
        PositionId: parseInt(document.getElementById("empPosId").value),
    };

    try {
        if (id) {
            const endDate = document.getElementById("empEndDate").value;
            body.EndDate = endDate || null;
            await apiRequest(`/Employees/${id}`, { method: "PUT", body });
            showInfo("Çalışan güncellendi.");
        } else {
            await apiRequest("/Employees", { method: "POST", body });
            showInfo("Çalışan oluşturuldu.");
        }
        resetEmpForm();
        await loadEmployees();
    } catch (err) { showError(err.message); }
}

async function deleteEmp(id) {
    if (!confirm("Bu çalışanı silmek istediğine emin misin?")) return;
    try {
        await apiRequest(`/Employees/${id}`, { method: "DELETE" });
        await loadEmployees();
    } catch (e) { showError(e.message); }
}

// ================= ONBOARDING ŞABLONLARI (Ik) =================
async function renderTemplates() {
    setContent(`
    <h2>Onboarding Şablonları</h2>
    <div class="error" id="actionMsg"></div>
    <div class="card">
      <h3 id="tplFormTitle">Yeni Şablon</h3>
      <form id="tplForm" class="form-grid">
        <input type="hidden" id="tplId" />
        <div><label>Ad</label><input id="tplName" required /></div>
        <div><label>Açıklama</label><input id="tplDesc" /></div>
        <div id="tplActiveWrap" class="hidden">
          <label>Aktif mi?</label>
          <select id="tplActive"><option value="true">Evet</option><option value="false">Hayır</option></select>
        </div>
        <div class="form-actions">
          <button type="submit">Kaydet</button>
          <button type="button" class="secondary" id="tplCancel">Vazgeç</button>
        </div>
      </form>
    </div>
    <div class="card">
      <table>
        <thead><tr><th>Id</th><th>Ad</th><th>Açıklama</th><th>Aktif</th><th></th></tr></thead>
        <tbody id="tplTableBody"></tbody>
      </table>
    </div>
    <div class="card" id="tplTasksCard" style="display:none">
      <h3 id="tplTasksTitle">Şablon Görevleri</h3>
      <form id="tplTaskForm" class="form-grid">
        <div><label>Başlık</label><input id="tplTaskTitle" required /></div>
        <div><label>Açıklama</label><input id="tplTaskDesc" /></div>
        <div><label>Sorumlu Departman Id</label><input id="tplTaskDeptId" type="number" /></div>
        <div><label>Kaç Gün İçinde</label><input id="tplTaskDueDays" type="number" required /></div>
        <div><label>Zorunlu mu?</label>
          <select id="tplTaskMandatory"><option value="true">Evet</option><option value="false">Hayır</option></select>
        </div>
        <div class="form-actions"><button type="submit">Görev Ekle</button></div>
      </form>
      <table>
        <thead><tr><th>Başlık</th><th>Açıklama</th><th>Departman</th><th>Süre (gün)</th><th>Zorunlu</th></tr></thead>
        <tbody id="tplTasksBody"></tbody>
      </table>
    </div>
  `);

    document.getElementById("tplCancel").addEventListener("click", resetTplForm);
    document.getElementById("tplForm").addEventListener("submit", saveTpl);
    document.getElementById("tplTaskForm").addEventListener("submit", addTplTask);
    await loadTemplates();
}

let currentTemplateId = null;

async function loadTemplates() {
    try {
        const list = await apiRequest("/OnboardingTemplates");
        document.getElementById("tplTableBody").innerHTML = list.map((t) => `
      <tr>
        <td>${t.id}</td>
        <td>${esc(t.name)}</td>
        <td>${esc(t.description)}</td>
        <td>${t.isActive ? "Evet" : "Hayır"}</td>
        <td class="row-actions">
          <button class="small" onclick="showTplTasks(${t.id}, '${escJs(t.name)}')">Görevler</button>
          <button class="small" onclick="editTpl(${t.id}, '${escJs(t.name)}', '${escJs(t.description)}', ${t.isActive})">Düzenle</button>
          <button class="small danger" onclick="deleteTpl(${t.id})">Sil</button>
        </td>
      </tr>`).join("");
    } catch (e) { showError(e.message); }
}

function editTpl(id, name, desc, active) {
    document.getElementById("tplFormTitle").textContent = "Şablon Düzenle";
    document.getElementById("tplId").value = id;
    document.getElementById("tplName").value = name;
    document.getElementById("tplDesc").value = desc;
    document.getElementById("tplActiveWrap").classList.remove("hidden");
    document.getElementById("tplActive").value = String(active);
}

function resetTplForm() {
    document.getElementById("tplFormTitle").textContent = "Yeni Şablon";
    document.getElementById("tplId").value = "";
    document.getElementById("tplForm").reset();
    document.getElementById("tplActiveWrap").classList.add("hidden");
}

async function saveTpl(e) {
    e.preventDefault();
    const id = document.getElementById("tplId").value;
    const name = document.getElementById("tplName").value;
    const description = document.getElementById("tplDesc").value;
    try {
        if (id) {
            const isActive = document.getElementById("tplActive").value === "true";
            await apiRequest(`/OnboardingTemplates/${id}`, { method: "PUT", body: { Name: name, Description: description, IsActive: isActive } });
            showInfo("Şablon güncellendi.");
        } else {
            await apiRequest("/OnboardingTemplates", { method: "POST", body: { Name: name, Description: description } });
            showInfo("Şablon oluşturuldu.");
        }
        resetTplForm();
        await loadTemplates();
    } catch (err) { showError(err.message); }
}

async function deleteTpl(id) {
    if (!confirm("Bu şablonu silmek istediğine emin misin?")) return;
    try {
        await apiRequest(`/OnboardingTemplates/${id}`, { method: "DELETE" });
        await loadTemplates();
    } catch (e) { showError(e.message); }
}

async function showTplTasks(id, name) {
    currentTemplateId = id;
    document.getElementById("tplTasksCard").style.display = "block";
    document.getElementById("tplTasksTitle").textContent = `"${name}" Şablonunun Görevleri`;
    await loadTplTasks();
}

async function loadTplTasks() {
    try {
        const list = await apiRequest(`/OnboardingTemplates/${currentTemplateId}/tasks`);
        document.getElementById("tplTasksBody").innerHTML = list.map((t) => `
      <tr>
        <td>${esc(t.title)}</td>
        <td>${esc(t.description)}</td>
        <td>${esc(t.responsibleDepartmentName) || t.responsibleDepartmentId || "-"}</td>
        <td>${t.dueInDays}</td>
        <td>${t.isMandatory ? "Evet" : "Hayır"}</td>
      </tr>`).join("");
    } catch (e) { showError(e.message); }
}

async function addTplTask(e) {
    e.preventDefault();
    if (!currentTemplateId) return;
    const deptRaw = document.getElementById("tplTaskDeptId").value;
    const body = {
        Title: document.getElementById("tplTaskTitle").value,
        Description: document.getElementById("tplTaskDesc").value,
        ResponsibleDepartmentId: deptRaw ? parseInt(deptRaw) : null,
        DueInDays: parseInt(document.getElementById("tplTaskDueDays").value),
        IsMandatory: document.getElementById("tplTaskMandatory").value === "true",
    };
    try {
        await apiRequest(`/OnboardingTemplates/${currentTemplateId}/tasks`, { method: "POST", body });
        document.getElementById("tplTaskForm").reset();
        showInfo("Görev eklendi.");
        await loadTplTasks();
    } catch (err) { showError(err.message); }
}

// ================= ONBOARDING SÜREÇLERİ (Ik) =================
async function renderProcesses() {
    setContent(`
    <h2>Onboarding Süreçleri</h2>
    <div class="error" id="actionMsg"></div>
    <div class="card">
      <h3>Yeni Süreç Başlat</h3>
      <form id="procForm" class="form-grid">
        <div><label>Çalışan Id</label><input id="procEmpId" type="number" required /></div>
        <div><label>Şablon Id</label><input id="procTplId" type="number" required /></div>
        <div class="form-actions"><button type="submit">Başlat</button></div>
      </form>
    </div>
    <div class="card">
      <table>
        <thead><tr><th>Id</th><th>Çalışan</th><th>Şablon</th><th>Başlangıç</th><th>Bitiş</th><th>Durum</th></tr></thead>
        <tbody id="procTableBody"></tbody>
      </table>
    </div>
  `);

    document.getElementById("procForm").addEventListener("submit", startProcess);
    await loadProcesses();
}

async function loadProcesses() {
    try {
        const list = await apiRequest("/OnboardingProcesses");
        document.getElementById("procTableBody").innerHTML = list.map((p) => `
      <tr>
        <td>${p.id}</td>
        <td>${esc(p.employeeName) || p.employeeId}</td>
        <td>${esc(p.onboardingTemplateName) || p.onboardingTemplateId}</td>
        <td>${fmtDate(p.startDate)}</td>
        <td>${fmtDate(p.endDate)}</td>
        <td>${PROCESS_STATUS_LABELS[p.status] || p.status}</td>
      </tr>`).join("");
    } catch (e) { showError(e.message); }
}

async function startProcess(e) {
    e.preventDefault();
    const body = {
        EmployeeId: parseInt(document.getElementById("procEmpId").value),
        OnboardingTemplateId: parseInt(document.getElementById("procTplId").value),
    };
    try {
        await apiRequest("/OnboardingProcesses", { method: "POST", body });
        showInfo("Süreç başlatıldı.");
        document.getElementById("procForm").reset();
        await loadProcesses();
    } catch (err) { showError(err.message); }
}

// ================= GÖREVLER (Ik, DepartmentUser) =================
async function renderTasks() {
    setContent(`
    <h2>Görevler</h2>
    <div class="error" id="actionMsg"></div>
    <div class="card">
      <table>
        <thead>
          <tr>
            <th>Başlık</th><th>Departman</th><th>Sorumlu</th><th>Bitiş</th>
            <th>Durum</th><th>Not</th><th></th>
          </tr>
        </thead>
        <tbody id="taskTableBody"></tbody>
      </table>
    </div>
  `);
    await loadTasks();
}

async function loadTasks() {
    try {
        const list = await apiRequest("/OnboardingTasks");
        document.getElementById("taskTableBody").innerHTML = list.map((t) => `
      <tr>
        <td>${esc(t.title)}${t.isMandatory ? ' <span class="badge">zorunlu</span>' : ""}</td>
        <td>${esc(t.responsibleDepartmentName) || t.responsibleDepartmentId || "-"}</td>
        <td>${esc(t.responsibleUserName) || t.responsibleUserId || "-"}</td>
        <td>${fmtDate(t.dueDate)}</td>
        <td>
          <select id="taskStatus_${t.id}">
            ${TASK_STATUS_OPTIONS.map((s) => `<option value="${s}" ${s === t.status ? "selected" : ""}>${TASK_STATUS_LABELS[s]}</option>`).join("")}
          </select>
        </td>
        <td>
          <textarea id="taskNote_${t.id}" rows="2" style="width:150px">${esc(t.note)}</textarea>
        </td>
        <td class="row-actions">
          <button class="small" onclick="saveTaskStatus(${t.id})">Durumu Kaydet</button>
          <button class="small secondary" onclick="saveTaskNote(${t.id})">Notu Kaydet</button>
          <button class="small secondary" onclick="toggleTaskHistory(${t.id})">Geçmiş</button>
          <div id="taskHistory_${t.id}" class="hidden muted"></div>
        </td>
      </tr>`).join("");
    } catch (e) { showError(e.message); }
}

async function saveTaskStatus(id) {
    const status = document.getElementById(`taskStatus_${id}`).value;
    try {
        await apiRequest(`/OnboardingTasks/${id}/status`, {
            method: "PUT",
            body: { NewStatus: status, ChangedByUserId: getUserId() },
        });
        showInfo("Durum güncellendi.");
        await loadTasks();
    } catch (e) { showError(e.message); }
}

async function saveTaskNote(id) {
    const note = document.getElementById(`taskNote_${id}`).value;
    try {
        await apiRequest(`/OnboardingTasks/${id}/note`, { method: "PUT", body: { Note: note } });
        showInfo("Not güncellendi.");
    } catch (e) { showError(e.message); }
}

async function toggleTaskHistory(id) {
    const box = document.getElementById(`taskHistory_${id}`);
    if (!box.classList.contains("hidden")) {
        box.classList.add("hidden");
        return;
    }
    try {
        const history = await apiRequest(`/OnboardingTasks/${id}/history`);
        box.innerHTML = history.length
            ? history.map((h) => `<div>${fmtDate(h.changedAt)} - ${esc(h.changedByUsername)}: ${TASK_STATUS_LABELS[h.oldStatus] || h.oldStatus} → ${TASK_STATUS_LABELS[h.newStatus] || h.newStatus}</div>`).join("")
            : "<div>Kayıt yok.</div>";
        box.classList.remove("hidden");
    } catch (e) { showError(e.message); }
}

init();