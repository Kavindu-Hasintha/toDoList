import axios from "axios";
import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

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

  return <div>Home</div>;
}

export default Home;
