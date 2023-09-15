/*import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';*/
import React, { useState } from 'react';
import './dist/style/custom.css';
import Sidebar from './components/layout/SideBar';
import Contents from './components/layout/Content';
import { Layout, theme } from 'antd';
const { Header } = Layout;



const App = () => {

    var [selectedPath, setSelectedPath] = useState('/home'); // Initial selected path
  var handleItemClick = (path) => {
    
    setSelectedPath(path);
  };
  const sidebarItems = [
    { label: 'صفحه اصلی', path: '/home',id:'m13' },
    { label: 'تست', path: '/FetchData',id:'m14' },
  ];

  const {
    token: { colorBgContainer },
  } = theme.useToken();
  return (
 

<Layout>
      <Header
        style={{
          display: 'flex',
          alignItems: 'center',
        }}
      >
      </Header>
      <Layout>
      <Sidebar items={sidebarItems} onItemClick={handleItemClick} bgColor={colorBgContainer} />
        <Layout
          style={{
            padding: '0 24px 24px',
          }}
        >
         <div class="container mx-auto px-4">
         <Contents path={selectedPath} />
         </div>
          
        </Layout>
      </Layout>
    </Layout>


 
  );
};
export default App;