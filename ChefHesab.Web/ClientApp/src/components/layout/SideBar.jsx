import React , { Component }from 'react';
import { Layout,Menu } from 'antd';

import {AppstoreOutlined} from '@ant-design/icons';
import { v4 as uuidv4 } from 'uuid';
import { Link } from 'react-router-dom';
const {Sider } = Layout;




 class Sidebar extends Component {

  constructor(){
   super()
   this.state = {
    collapsed: false,
   
  };  
   this.rootSubmenuKeys=[] 
  }
  renderchild=(child,handleOnClick)=>{
console.log(handleOnClick)
    var childresult=[];
    child.map(item=> childresult.push({
      label:(
        <Link key={uuidv4()} 
              onClick={() =>handleOnClick(item.path)}
              
        >
                  {item.label}
        </Link>
      ),    

      key:uuidv4()}));
    return childresult;


    
        
     }
  
  render() {
    return (
      <Sider 
       collapsible  
       collapsed={this.state.collapsed} 
       onCollapse={(value) => this.setState({ collapsed:value})}      
       theme='dark'
       style={{     
        height:'90vh'
      }}
          > 
      <Menu   theme="dark"  // defaultSelectedKeys={['1']}
                     mode="inline"                
                      
                     
                    items={[
                      {
                        icon:<AppstoreOutlined/> , 
                          label: 'تعاریف پایه', 
                          path: '/items/list',
                          key: uuidv4(),  
                          children:this.renderchild(this.props.items,this.props.onItemClick),
                                           
                      }
                    ]}   
                >
                 
      </Menu>
    </Sider>
  
    );
  }
}



export default Sidebar;
