/*
[一、 basic, blend,_Time,简单的纹理样式]
你好，这里是一个shader基础教程的笔记本，在项目中长久存在但其实是个独立的笔记，没有工程作用
我强推油管的姐姐！https://www.youtube.com/watch?v=kfM-yu0iQBk 这是那个网址
知识点如下：
0.如果想要拥有自动补全和查找函数定义的功能，可以去unity assest商店搜索shaderlabvs pro，支付宝支付200多购买，草我为什么要买pro，见鬼

1.properties是暴露给unity editor修改的一种东西
2.Subshader中，有vertex顶点shader 和fragment 片元（pixel）shader，分别对应了函数vert和frag
3.渲染顺序从顶点开始到像素为止，中间v2f结构被修改成了Interpolator，意为插值，的确是通过插值来映射顶点到像素
4.MeshData(原appdata)中的NORMAL(MESH的法线),POSITION（Mesh的坐标位置）,TEXCOORD0（uv coordinate）,TEXCOORD1（light map）都是有实际意义的规定值, i.uv1.x应该是一个从0到1的数
  Interpolators 中的则没啥意义，你随意
5.因为是meshdata所以拿来的坐标和uv和normal都是物体本身的，需要o.normals = UnityObjectToWorldNormal( v.normals );才能转换成世界坐标
6.怎么做双色渐变？首先，(i.uv.xxx,1)可以形成从(0,0,0,1)到（1，1，1，1）的渐变，用做lerp的第三个值（用来插的那个）然后是公式(left+(right - left)*(uv.x))
7.函数lerp（colorA，colorB，lerp）中ColorA对应float4，非常认真的写了四次重载
8.【DEBUG】shader不能debug也不能log，你只能通过设置frag的return确认自己写了个啥玩意儿
9.【渐变陡度函数gradient】我们来写一个InverseLerp，这是一个常用的调整渐变陡度的函数，
10.【一个易错点】出现黑白色首先查看是否有float4 写成了float
11.【区间界限】即使用了Colorstart和Colorend，也难免超过[0，1]范围，这是因为InverseLerp函数让0-1区间变化变小了，我们可以用frac（取小数函数）做出条纹状，我们也可以用clamp限定它们的边界，只做出一条
    但是shader里面clamp的函数不叫clamp，叫做saturate(t)，返回值在[0,1]范围下的t（跪求一双没有看过这玩意中文的眼睛，为啥是饱和啊哦透）
12.有趣的是这些frac函数不是unity的，你应该去 https://developer.download.nvidia.com/cg/index_stdlib.html 查看文档，搜索nivdia cg就行
13.smoothstep(float  a, float  b, float  x)函数完成了上述插值到clamp的工作，但是渐变曲线长这样t*t*(3.0 - (2.0*t))，可以去 https://www.desmos.com/calculator?lang=zh-TW 这个网站验证y = t*t*(3.0-(2.0*t)){0<t<1}到底长什么样，我们手写的是y = x的渐变曲线
14.【没有悬崖边界的渐变】可以使用cos，也可以使用abs，具体方法自己想，借助一下数学工具
15.subshader 有自己的tags ，pass也有自己的tags
16.CGPROGRAM 和ENDCG 包裹了hlsl部分，之外的部分都是unity的封装
17:斜向纹理float color = cos((i.uv1.y +  i.uv1.x )* 25) * 0.5 + 0.5;//返回t的小数部分
18.z形纹理 float color = cos((offset +  i.uv1.x )* 25) * 0.5 + 0.5;    float offset = cos(i.uv1.y * 25) *0.01 + 0.01;
19.unity 提供了一个全局变量_Time。_Time.y表示秒数，.w表示second/20，可以通过引用决定时间变化快慢
20.如果想在Sence界面查看动画，记得去勾选always refresh ，在Sence和pLAY下面有一个下拉菜单，里面有这个选项
21.我们做一个渐变特效吧！
22.
                float xoffset = cos(i.uv1.y * 25) *0.01 + 0.01;
                float t = cos((i.uv1.y + xoffset +  _Time.y * 0.2 )* 25 ) * 0.5 + 0.5;//返回t的小数部分
                t *= 1 - i.uv1.y ;
23.在blend阶段[一般而言也是最后的渲染步骤，也叫做additive blending]，frag函数return的color被叫做source color （src）
   我们需要一个背景，作为render的 destination，也就是destination(dst)
   additive blending  = src * A +/- dst * B;
   additive blending 一般用在光特效
   这个步骤并不在shader code里面，它在shaderlab里面，所以在CGPROGRAM的外面
24.render queue 是一个很机械的队列，首先渲染skybox，然后geometry，然后所有的additive transparent，然后覆盖的overlays
    比如flares
25.要处理透明物体，就该把tags改成
    "RenderType" = "Transparent"
    "Queue" = "Transparent"
26.zwrite off，ztest
    详细可以搜索csdn也很详细，首先深度缓存存的是一个像素要绘制在屏幕上的深度值
    如果zwrite on(默认值on)就启用这个缓存区，那么绘制前将会比较【当前处理的像素深度值】和【zbuffer里面的深度值】，浅的将会写入缓冲区
    在zwrite和ztest之前，先把渲染队列中比较前的渲染了
    ztest 就是启用深度测试，LEqual（默认）通过比较深度改写颜色 Off关闭深度测试 Always直接写进颜色缓冲区
    Never不使用这个颜色，直接消失
    如果这两个值都是默认值：ZWrite On ZTest LEqual那么可以见得
    1.当ZWrite为On时，ZTest通过时，该像素的深度才能成功写入深度缓存，同时因为ZTest通过了，该像素的颜色值也会写入颜色缓存。
    2.当ZWrite为On时，ZTest不通过时，该像素的深度不能成功写入深度缓存，同时因为ZTest不通过，该像素的颜色值不会写入颜色缓存。
    3.当ZWrite为Off时，ZTest通过时，该像素的深度不能成功写入深度缓存，同时因为ZTest通过了，该像素的颜色值会写入颜色缓存。
    4.当ZWrite为Off时，ZTest不通过时，该像素的深度不能成功写入深度缓存，同时因为ZTest不通过，该像素的颜色值不会写入颜色缓存。
    ZTest GEqual 只渲染被挡住的部分（less Equal就是渲染挡住别人的部分）,很有用，一般有两个shader一个用来渲染挡住别人的时候，一个用来渲染被别人挡住的时候
27.如何才能把半透明物体的背面也渲染出来？
    其实一般而言物体背面之所以没有渲染是因为他们把背面删掉了。Cull Back是默认值，表示Back（背面）是被删掉（Cull）的
28.RenderType 渲染管线中当前shader类型
   Queue 改变渲染队列的顺序，一个transparent类型shader的将会在所有opaque不透明物体之后渲染
29.怎么去掉上下两个面？
    return t * (abs(i.normal.y) < 0.999);
30.这次写个更好看的渐变透明shader
                float xoffset = cos(i.uv1.y * 25) *0.01 + 0.01;
                float t = cos((i.uv1.y + xoffset +  _Time.y * 0.2 )* 25 ) * 0.5 + 0.5;
                t *= 1 - i.uv1.y ;

                float4 topButtomRemover = (abs(i.normal.y) < 0.999);
                float waves = t * topButtomRemover;
                
                float4 gradient = lerp(_ColorA, _ColorB, i.uv1.y);
                return gradient * waves;//因为shader里面true false用了01表示所以可以这么做
31.vertex offset
    我们在vert函数里面做形变！
      v.vertex.y = waves * _WaveAmp;
    如何形成两个方向的波浪？
      float waves = cos((v.uv0.y -  _Time.y * 0.1 )* 25 ) ;
      float waves2 = cos((v.uv0.x -  _Time.y * 0.1 )* 25 ) ;
      v.vertex.y = waves * waves2 * _WaveAmp;
    中间来个点？
      float2 uvsCentered = i.uv1 *2 - 1;
      float radiaDistance = length(uvsCentered);
      return float4(radiaDistance.xxx, 1);//因为shader里面true false用了01表示所以可以这么做
    
     
*/



Shader "Unlit/Shader1"
{
    Properties
    {//这里是 input data
//        _MainTex ("Texture", 2D) = "white" {}//_MainTex需要在下面定义
        _ColorA ("Color A" , Color) = (1, 1, 1, 1)
        _ColorB ("Color B" , Color) = (0, 0, 0, 0)//用 0 代替 (0, 0, 0, 0)是不可以的，shader properties对类型定义是死板的,但是在subshader的时候又允许float = float4，frag的rturn也可以把float当成（float ，float， float，float），真实迷惑
        _Scale ("UV Scale", Float) = 1  //_Scale 本文件内变量名， UV Scale 给unity editor的人看的名 ，Float 类型，1 默认值
        _WaveAmp ("Wave Amplitude", Range(0, 1)) = 1
    }
    SubShader
    { 
        Tags { 
            "RenderType" = "Opaque" // 告诉渲染管线，这是个什么类型
            // "RenderType" = "Transparent"
            // "Queue" = "Transparent"
        }
       // LOD 100 //不太重要，油管姐姐删掉了

        Pass
        {

           // Blend DstColor Zero // [src * dst + dst * 0]
           //  Blend One One
           // ZWrite Off
           // ZTest GEqual

            CGPROGRAM
            #pragma vertex vert //vertex shader在这里用vert函数表示，下面有对应函数
            #pragma fragment frag
            // make fog work
            //    #pragma multi_compile_fog  //不重要，她删掉了
            //下面很像C语言
            #include "UnityCG.cginc"//一个很重要的文件，有很有用的函数

             //        sampler2D _MainTex; //在上面已赋值
            float4 _MainTex_ST;
            float4 _Color;//这样一个被properties声明的变量，在vertex shader和fragment shader都能用，非常方便
            float _Scale;
            float4 _ColorA;
            float4 _ColorB;
            float _WaveAmp;

            struct MeshData // 姐姐觉得这个名字好！per-vertex mesh  data
            {
                float4 vertex : POSITION;// pre-vertex POSITION  ，前面的vertex是变量，可以随意改变，后面POSITION表示位置信息将传递到这个变量里
                float3 normals : NORMAL;// 这里的NORMAL坐标是物体坐标，不是世界坐标，意味着转动球体，(0,1,0)的点有且只有一个且会随之转动
                float2 uv0 : TEXCOORD0;// uv 坐标，一般用在mapping texture上 ，来自uv channel的数据将传递到这里,虽然0和1都是一样的，但是可以给他们不同的功能，diffuse，normal
                float2 uv1 : TEXCOORD1;// light map
                // float4 color : COLOR;// vertex color
                // float4 tangent : TANGENT; // tangent direction (x,y,z) tangent sign (w)
            };

            struct Interpolators // 从vertex shader 到 fragement shader，传递的数据，可以随便改名称，这里从v2f变成中间值
            {// 其实interpolator的含义是插值，从vertex到pixel之间确实是过渡插值的方法（类似alpha blend）
                float3 normal : TEXCOORD0;// 姐姐说这个和uv channel没有关系，你随意,仅仅代表一个从vert到frag的数据流
                float2 uv1 :TEXCOORD1;// 这样也行
              //  UNITY_FOG_COORDS(1)//去掉，我们今天不管fog
                float4 vertex : SV_POSITION; // clip space postion 裁剪空间坐标，视锥体剔除视野外的东西称之为裁剪，裁剪空间就是视锥体（能看到的）空间，这里是每个顶点的裁剪空间坐标

            };

            // bool 0 1类似C，通用

            // float (32 bit float)最精确，pc上用这个多
            // half (16 bit float)
            // fixed (lower precision) [  -1, 1]
            // float4 half4 fixed4
            // float4x4 就是4*4矩阵
            Interpolators vert (MeshData v)//这里也要改，vert 返回就是vertex shader的输出,v由unity自动填充
            {
                Interpolators o;//output的意思

                float waves = cos((v.uv0.y -  _Time.y * 0.1 )* 25 ) ;
                float waves2 = cos((v.uv0.x -  _Time.y * 0.1 )* 25 ) ;

               // v.vertex.y = waves * waves2 * _WaveAmp;


                o.vertex = UnityObjectToClipPos( v.vertex );//local space to clip space
                o.normal =  v.normals ;//转换成世界坐标,当然，vert比pixel少，所以计算在vert里做更好
                o.uv1 = v.uv0 * _Scale; //小知识，scale 大于1的时候，颜色越界但是仍然取最大值1，是合法的值
                /* o.uv = TRANSFORM_TEX(v.uv, _MainTex); */
            //    UNITY_TRANSFER_FOG(o,o.vertex);//去掉，今天我们不管
                return o;
            }

            float InverseLerp(float a, float b, float v) {
                return (v - a)/(b - a);   
             }

            fixed4 frag (Interpolators i) : SV_Target//表示将会输出到frame buffer
            {

                //float4 myValue;
                
            //    float t = InverseLerp(_ColorStart, _ColorEnd, i.uv1.x);

              //  float4 outColor = (t, 0, 0, 1);
              //  float2 otherValue = myValue.rg;//red & green，对于一个float4，value.xyzw和rgba指的都是float的4个值（1,2,3,4）
                //如果float2 otherValue = myValue.gr;那就是把myValue的green值赋给otherValue的red值
                //也就是所谓的swizzling(旋转)

                float2 uvsCentered = i.uv1 *2 - 1;
                float radiaDistance = length(uvsCentered);

                return float4(radiaDistance.xxx, 1);//因为shader里面true false用了01表示所以可以这么做
            }
            ENDCG
        }
    }
}
