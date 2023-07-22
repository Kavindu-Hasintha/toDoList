import React, { useEffect, useState } from "react";
import TextField from "@mui/material/TextField";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import Modal from "react-bootstrap/Modal";

function Setting(props) {
  const navigate = useNavigate();
  const [userData, setUserData] = useState({
    id: 0,
    name: "",
    email: "",
    password: "",
  });
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [oldPassword, setOldPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [reNewPassword, setReNewPassword] = useState("");
  const [show, setShow] = useState(false);
  const [actionNo, setActionNo] = useState(0);

  const backToHome = () => {
    navigate(-1);
  };

  useEffect(() => {
    axios
      .get("https://localhost:7068/api/Users/getuserdetails" + props.userId)
      .then((res) => {
        setUserData(res.data);
        setName(res.data.name);
        setEmail(res.data.email);
      })
      .catch((err) => {
        toast.error("Can not reload this page.");
      });
  }, []);

  const handleChangeDetails = () => {
    if (userData.name !== name || userData.email !== email) {
      axios
        .put("https://localhost:7068/api/Users", {
          id: userData.id,
          name: name,
          email: email,
          password: userData.password,
        })
        .then((res) => {
          toast.success(res.data);
          if (props.userType === "0") {
            window.location.reload(false);
          } else {
            navigate(-1);
          }
        })
        .catch((err) => {
          toast.error("Update failed.");
        });
    }
  };

  const handleChangePassword = () => {
    if (
      oldPassword.length === 0 ||
      newPassword.length === 0 ||
      reNewPassword.length === 0
    ) {
      toast.error("Please fill all the fields.");
    } else if (userData.password !== oldPassword) {
      toast.error("Invalid password.");
    } else if (newPassword !== reNewPassword) {
      toast.error("Re-entered new password is not matching.");
    } else {
      axios
        .put("https://localhost:7068/api/Users", {
          id: userData.id,
          name: userData.name,
          email: userData.email,
          password: newPassword,
        })
        .then((res) => {
          if (props.userType === "0") {
            window.location.reload(false);
          } else {
            navigate(-1);
          }
          toast.success(res.data);
        })
        .catch((err) => {
          toast.error("Update failed.");
        });
    }
  };

  const handleDeleteAccount = () => {
    axios
      .delete("https://localhost:7068/api/Users/" + userData.id)
      .then((res) => {
        toast.success(res.data);
        if (props.userType === "0") {
          navigate("/signup");
        } else {
          navigate(-1);
        }
      })
      .catch((err) => {
        toast.error("Deletion failed.");
      });
  };

  const handleConfirm = () => {
    setShow(false);
    if (actionNo === 1) {
      handleChangeDetails();
    } else if (actionNo === 2) {
      handleDeleteAccount();
    } else if (actionNo === 3) {
      handleChangePassword();
    }
    setActionNo(0);
  };

  const changeDetails = () => {
    setActionNo(1);
    setShow(true);
  };

  const changePassword = () => {
    setActionNo(3);
    setShow(true);
  };

  const deleteAccount = () => {
    setActionNo(2);
    setShow(true);
  };

  const handleClose = () => {
    setShow(false);
  };

  return (
    <div className="container bg-white w-50">
      <Modal show={show} onHide={handleClose}>
        <Modal.Header closeButton>
          <Modal.Title>Confirmation</Modal.Title>
        </Modal.Header>
        <Modal.Body>Are you sure?</Modal.Body>
        <Modal.Footer>
          <button className="btn btn-secondary" onClick={handleClose}>
            No
          </button>
          <button className="btn btn-primary" onClick={handleConfirm}>
            Yes
          </button>
        </Modal.Footer>
      </Modal>
      <div className="row d-flex align-items-center py-2 border-bottom border-dark">
        <div className="col-8">
          <h2 className="text-start ps-4">Setting</h2>
        </div>
        <div className="col-4 d-flex justify-content-end">
          <button
            type="button"
            className="btn btn-primary me-4 px-4"
            onClick={backToHome}
          >
            Home
          </button>
        </div>
      </div>
      <div className="row">
        <div className="container my-4 w-auto border border-dark rounded">
          <div className="row py-4">
            <div className="col-6">
              <TextField
                id="outlined-basic"
                label="Name"
                variant="outlined"
                name="name"
                value={name}
                onChange={(e) => setName(e.target.value)}
              />
            </div>
            <div className="col-6">
              <TextField
                id="outlined-basic"
                label="Email"
                variant="outlined"
                name="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />
            </div>
          </div>
          <div className="row pb-4">
            <div className="col-6"></div>
            <div className="col-6">
              <button
                className="btn btn-warning w-50 px-2 py-2"
                onClick={changeDetails}
              >
                Save
              </button>
            </div>
          </div>
        </div>
      </div>
      <div className="row">
        <div className="container mb-4 w-auto border border-dark rounded">
          <div className="row py-4">
            <div className="col-6">
              <TextField
                id="outlined-password-input"
                label="Old Password"
                type="password"
                name="oldPassword"
                onChange={(e) => {
                  setOldPassword(e.target.value);
                }}
              />
            </div>
            <div className="col-6"></div>
          </div>
          <div className="row pb-4">
            <div className="col-6">
              <TextField
                id="outlined-password-input"
                label="New Password"
                type="password"
                name="newPassword"
                onChange={(e) => {
                  setNewPassword(e.target.value);
                }}
              />
            </div>
            <div className="col-6">
              <TextField
                id="outlined-password-input"
                label="Re-New Password"
                type="password"
                name="reNewPass"
                onChange={(e) => {
                  setReNewPassword(e.target.value);
                }}
              />
            </div>
          </div>
          <div className="row pb-4">
            <div className="col-6">
              {userData.id !== 1 && (
                <button
                  className="btn btn-danger w-auto"
                  onClick={deleteAccount}
                >
                  Delete My Account
                </button>
              )}
            </div>
            <div className="col-6">
              <button
                className="btn btn-warning w-auto"
                onClick={changePassword}
              >
                Change Password
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Setting;
