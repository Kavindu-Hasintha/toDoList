import axios from "axios";
import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import Header from "./Header";
import Footer from "./Footer";
import HomeStyles from "../Styles/Home.module.css";
import TodoTable from "./TodoTable";

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
        console.log(err);
      });
  }, []);

  const handleSignupPage = () => {
    navigate("/");
  };

  return (
    <div className={HomeStyles.backgroundImage}>
      <Header
        name={userData.name}
        onHandleTopButton={handleSignupPage}
        pageId="0"
      />
      <div className="my-4">
        <TodoTable userId={userId} />
      </div>

      <Footer />
    </div>
  );
}

export default Home;
