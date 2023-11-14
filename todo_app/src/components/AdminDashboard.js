import React, { useEffect, useState } from "react";
import Header from "./Header";
import { Route, Routes, useNavigate } from "react-router-dom";
// import axios from "axios";
import { toast } from "react-toastify";
import Footer from "./Footer";
import AdminDashboardStyles from "../Styles/AdminDashboard.module.css";
import UsersTable from "./UsersTable";
import Setting from "./Setting";
import TodoTable from "./TodoTable";
import useHttp from "../hooks/use-http";

function AdminDashboard() {
  const navigate = useNavigate();
  const [userData, setUserData] = useState({
    name: "",
    email: "",
    password: "",
  });
  const [userId, setUserId] = useState(0);

  const { isLoading, error, sendRequest: fetchAdminData } = useHttp();

  useEffect(() => {
    fetchAdminData(
      {
        url: "https://localhost:7068/api/Users/1",
      },
      setUserData
    );

    // axios
    //   .get("https://localhost:7068/api/Users/1")
    //   .then((res) => {
    //     setUserData(res.data);
    //   })
    //   .catch((err) => {
    //     toast.error("Can not load this page.");
    //   });
  }, [fetchAdminData]);

  const handleLogOut = () => {
    navigate("/");
  };

  const getUserId = (id) => {
    setUserId(id);
  };

  return (
    <div className={AdminDashboardStyles.backgroundImage}>
      {isLoading && <div>Loading...</div>}
      {error && <div>{toast.error("Can not load this page.")}</div>}
      {!isLoading && !error && (
        <div>
          <Header
            name={userData.name}
            onHandleLogOut={handleLogOut}
            pageId="2"
          />

          <div className="my-4">
            <Routes>
              <Route
                path="/"
                element={<UsersTable onGetUserId={getUserId} />}
              />
              <Route path="setting" element={<Setting userId="1" />} />
              <Route
                path="usertodolist"
                element={<TodoTable userId={userId} userType="1" />}
              />
              <Route
                path="usersetting"
                element={<Setting userId={userId} userType="1" />}
              />
            </Routes>
          </div>

          <Footer />
        </div>
      )}
    </div>
  );
}

export default AdminDashboard;
