import axios from "axios";
import React, { useEffect, useState } from "react";
import { Route, Routes, useNavigate } from "react-router-dom";
import Header from "./Header";
import Footer from "./Footer";
import HomeStyles from "../Styles/Home.module.css";
import TodoTable from "./TodoTable";
import Setting from "./Setting";

function Home() {
  const navigate = useNavigate();
  const [userId, setUserId] = useState(window.location.pathname.split("/")[1]);
  const [userData, setUserData] = useState({
    name: "",
    email: "",
    password: "",
  });

  useEffect(() => {
    axios
      .get("https://localhost:7068/api/Users/" + userId)
      .then((res) => {
        setUserData(res.data);
      })
      .catch((err) => {
        console.log("Error in Home.js");
      });
  }, []);

  const handleLogOut = () => {
    navigate("/");
  };

  return (
    <div className={HomeStyles.backgroundImage}>
      <Header name={userData.name} onHandleLogOut={handleLogOut} pageId="2" />
      <div className="my-4">
        <Routes>
          <Route
            path="/"
            element={<TodoTable userId={userId} userType="0" />}
          />
          <Route
            path="setting"
            element={<Setting userId={userId} userType="0" />}
          />
        </Routes>
      </div>

      <Footer />
    </div>
  );
}

export default Home;
