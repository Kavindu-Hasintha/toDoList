import axios from "axios";
import React, { useEffect, useState } from "react";
import { toast } from "react-toastify";
import TableStyles from "../Styles/TodoTable.module.css";
import { useNavigate } from "react-router-dom";

function UsersTable(props) {
  const navigate = useNavigate();
  const [usersData, setUsersData] = useState([
    {
      id: 0,
      name: "",
      email: "",
      password: "",
    },
  ]);

  useEffect(() => {
    axios
      .get("https://localhost:7068/api/Users")
      .then((res) => {
        setUsersData(res.data);
      })
      .catch((err) => {
        toast.error("Can not load this page.");
      });
  }, []);

  const openTodoList = (id) => {
    props.onGetUserId(id);
    navigate("usertodolist");
  };

  const openSetting = (id) => {
    props.onGetUserId(id);
    navigate("usersetting");
  };

  return (
    <div className="container bg-white w-75">
      <div className="row d-flex align-items-center py-2 border-bottom border-dark">
        <div className="col-8">
          <h2 className="text-start ps-4">Users List</h2>
        </div>
      </div>
      <div className="row p-3">
        <div className={TableStyles.tableDiv}>
          <table className="table table-hover">
            <thead>
              <tr>
                <th scope="col">Id</th>
                <th scope="col">Name</th>
                <th scope="col">Email</th>
                <th scope="col">Password</th>
                <th scope="col">Action</th>
              </tr>
            </thead>
            <tbody>
              {usersData.map((data) => {
                return (
                  <tr key={data.id}>
                    <td>{data.id}</td>
                    <td>{data.name}</td>
                    <td>{data.email}</td>
                    <td>{data.password}</td>
                    <td>
                      <button
                        className="btn btn-warning"
                        onClick={() => {
                          openTodoList(data.id);
                        }}
                      >
                        toDo List
                      </button>
                      <button
                        className="btn btn-danger ms-3"
                        onClick={() => {
                          openSetting(data.id);
                        }}
                      >
                        Setting
                      </button>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}

export default UsersTable;
