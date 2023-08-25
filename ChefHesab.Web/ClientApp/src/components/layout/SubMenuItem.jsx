import React ,{Component}from 'react';
import { v4 as uuidv4 } from 'uuid';
import { UserOutlined, } from '@ant-design/icons';
import {  Menu } from 'antd';
import { Link } from 'react-router-dom';


 class SubMenuItem extends Component {



    render() {
      return (
        <Menu.Item key={uuidv4()}  icon={<UserOutlined/>}>
        <Link  to={this.props.itemMenu.path}>
            <span className="nav-text">{this.props.itemMenu.label}</span>
        </Link>
        </Menu.Item>
    
      );
    }
  }
  
  export default SubMenuItem;