import React from "react";
import SettingsIcon from "@mui/icons-material/Settings";
import { useNavigate } from "react-router-dom";

const Header = (props) => {
  const navigate = useNavigate();

  const handleSetting = () => {
    navigate("setting");
  };

  return (
    <div className="container bg-transparent pt-4 d-flex justify-content-between align-items-center">
      <h2 className="text-white fs-1 font-weight-normal">
        {props.name === "" ? "toDo List" : props.name}
      </h2>
      <div className="d-flex">
        {props.name === "" ? (
          ""
        ) : (
          <button
            className="btn btn-outline-info py-2 px-3 me-3 fw-bold"
            onClick={handleSetting}
          >
            <SettingsIcon /> Settings
          </button>
        )}

        {props.pageId === "0" ? (
          <button
            className="btn btn-outline-info py-2 px-3 fw-bold"
            onClick={props.onHandleTopButton}
          >
            Sign Up
          </button>
        ) : (
          ""
        )}

        {props.pageId === "1" ? (
          <button
            className="btn btn-outline-info py-2 px-3 fw-bold"
            onClick={props.onHandleTopButton}
          >
            Sign In
          </button>
        ) : (
          ""
        )}

        {props.pageId === "2" ? (
          <button
            className="btn btn-outline-info py-2 px-3 fw-bold"
            onClick={props.onHandleLogOut}
          >
            Log Out
          </button>
        ) : (
          ""
        )}
      </div>
    </div>
  );
};

export default Header;
